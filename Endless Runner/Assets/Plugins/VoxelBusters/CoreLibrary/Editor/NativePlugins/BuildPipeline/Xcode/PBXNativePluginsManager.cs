#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public class PBXNativePluginsManager : NativePluginsManager, IPostprocessBuildWithReport
    {
#region Constants

        private const string                kPluginRelativePath     = "VoxelBusters/";
        
        private static readonly string      kPreprocessorFilePath   = NativePluginsPackageLayout.IosPluginPath + "/NPConfig.h";
        
        private static readonly string[]    kIgnoreFileExtensions   = new string[]
        {
            ".meta", 
            ".pdf",
            ".DS_Store",
            ".mdown",
            ".asset",
            ".cs"
        };

#endregion

#region Fields

        private     PBXProject                      m_project;
        
        private     List<string>                    m_librarySearchPaths;
        
        private     List<string>                    m_frameworkSearchPaths;
        
        private     PBXNativePluginsProcessor[]     m_pluginsProcessors;
        
#endregion

#region Properties

        public PBXProject Project => ObjectHelper.CreateInstanceIfNull(
            ref m_project,
            () =>
            {
                PBXProject  project = null;
                if (IsPostprocessing)
                {
                    project = new PBXProject();
                    project.ReadFromFile(ProjectFilePath);
                }
                return project;
            });

        private PBXNativePluginsProcessor[] PluginsProcessors => ObjectHelper.CreateInstanceIfNull(
            ref m_pluginsProcessors,
            () => FindPluginsProcessors<PBXNativePluginsProcessor>(this));

        private PBXNativePluginsExporterObject[] ActiveExporterObjects { get; set; }

        private string ProjectFilePath => PBXProject.GetPBXProjectPath(OutputPath);

#endregion

#region Base class methods

        protected override bool IsSupported(BuildTarget target)
        {
            return (BuildTarget.iOS == target) || (BuildTarget.tvOS == target);
        }

        protected override void OnPreprocessNativePlugins()
        {
            // Send message to complete preprocess actions to all the NativeProcessors
            var     linkerFileWriter    = CreateDefaultLinkXmlWriter();
            PluginsProcessors.ForEach(
                (item) =>
                {
                    item.OnUpdateExporterObjects();
                    item.OnUpdateLinkXml(linkerFileWriter);
                });
            linkerFileWriter.WriteToFile();
        }

        protected override void OnPostprocessNativePlugins()
        {
            // Set properties
            m_librarySearchPaths        = new List<string>();
            m_frameworkSearchPaths      = new List<string>();
            ActiveExporterObjects       = NativePluginsExporterObject.FindObjects<PBXNativePluginsExporterObject>();

            ClearPluginsData();
            UpdateProjectConfiguation();
        }

#endregion

#region Public methods

        public void AddFile(string sourceFilePath, string parentGroup, string[] compileFlags)
        {
            AddFileToProject(
                Project,
                sourceFilePath,
                Project.GetFrameworkGuid(),
                parentGroup,
                compileFlags);
        }

#endregion

#region Private methods

        private void UpdateProjectConfiguation()
        {
            LinkNativeAssets();
            UpdateInfoPlistDocument();
            UpdateProjectCapabilities();
            UpdateMacroDefinitions();

            // Apply changes
            File.WriteAllText(ProjectFilePath, Project.WriteToString());
        }

        private void LinkNativeAssets()
        {
            DebugLogger.Log(CoreLibraryDomain.Default, "Linking native files.");

            // Open project file for editing
            string  projectFilePath     = ProjectFilePath;
            var     project             = Project;
            string  mainTargetGuid      = project.GetMainTargetGuid();
            string  frameworkTargetGuid = project.GetFrameworkGuid();
            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Project File Path: {projectFilePath} targetGuid: {frameworkTargetGuid} ProjectPath: {OutputPath}");

            //@@ fix for "does not refer to a file in a known build section"
            project.AddSourcesBuildPhase(frameworkTargetGuid);
            project.AddHeadersBuildPhase(frameworkTargetGuid);

            // Read exporter settings for adding native files 
            foreach (var exporterObject in ActiveExporterObjects)
            {
                DebugLogger.Log(CoreLibraryDomain.Default, $"Is feature: {exporterObject.name} enabled:{exporterObject.IsEnabled}.");
                string  exporterFilePath    = Path.GetFullPath(AssetDatabase.GetAssetPath(exporterObject));
                string  exporterFolder      = Path.GetDirectoryName(exporterFilePath);
                string  exporterGroup       = GetExportGroupPath(exporterObject: exporterObject, prefixPath: kPluginRelativePath);
                string  parentFolder        = IOServices.GetDirectoryName(AssetDatabase.GetAssetPath(exporterObject));

                // Add files
                AddFolderToProject(project, parentFolder, frameworkTargetGuid, exporterGroup, exporterObject.CompileFlags);

                // Add headerpaths
                foreach (var pathInfo in exporterObject.HeaderPaths)
                {
                    string  destinationPath = GetFilePathInProject(pathInfo.AbsoultePath, exporterFolder, exporterGroup);
                    string  formattedPath   = FormatFilePathInProject(destinationPath);
                    project.AddHeaderSearchPath(frameworkTargetGuid, formattedPath);
                }

                // Add frameworks
                foreach (var framework in exporterObject.Frameworks)
                {
                    if (framework.Target.HasFlag(PBXTargetMembership.UnityIphone))
                    {
                        project.AddFrameworkToProject(mainTargetGuid, framework.Name, framework.IsOptional);
                    }
                    if (framework.Target.HasFlag(PBXTargetMembership.UnityFramework))
                    {
                        project.AddFrameworkToProject(frameworkTargetGuid, framework.Name, framework.IsOptional);
                    }
                }

                // Add build properties
                foreach (var property in exporterObject.BuildProperties)
                {
                    project.AddBuildProperty(frameworkTargetGuid, property.Key, property.Value);
                }
            }

            // Add header search paths
            foreach (string path in m_librarySearchPaths)
            {
                project.AddLibrarySearchPath(frameworkTargetGuid, FormatFilePathInProject(path));
            }

            // Add framework search paths
            foreach (string path in m_frameworkSearchPaths)
            {
                project.AddFrameworkSearchPath(frameworkTargetGuid, FormatFilePathInProject(path));
            }

            // Add resources
            CopyAssetsFromStreamingAssets();

            // Send message to all the NativeProcessors
            PluginsProcessors.ForEach(
                (item) =>
                {
                    item.OnAddFiles();
                    item.OnAddFolders();
                    item.OnAddResources();
                    item.OnUpdateConfiguration();
                });
        }

        private string GetExportGroupPath(NativePluginsExporterObject exporterObject, string prefixPath)
        {
            string  groupPath               = prefixPath;
            bool    usesNestedHierarchy     = true;
            if (exporterObject.Group != null)
            {
                groupPath                  += exporterObject.Group.Name + "/";
                usesNestedHierarchy         = exporterObject.Group.UsesNestedHeierarchy;
            }
            if (usesNestedHierarchy)
            {
                groupPath                  += exporterObject.name + "/";
            }
            return groupPath;
        }

        private void AddFileToProject(PBXProject project, string sourceFilePath, string targetGuid, string parentGroup, string[] compileFlags)
        {
            // Convert relative path to absolute path
            if (!Path.IsPathRooted(sourceFilePath))
            {
                sourceFilePath          = Path.GetFullPath(sourceFilePath);
            }

            // Copy the file to the project directory
            string  fileName            = Path.GetFileName(sourceFilePath);
            string  destinationFilePath = sourceFilePath;
            if (!IOServices.IsSubDirectory(OutputPath, destinationFilePath))
            {
                string  destinationFolder   = IOServices.CombinePath(OutputPath, parentGroup);
                destinationFilePath         = CopyFileToProject(sourceFilePath, destinationFolder);
                DebugLogger.Log(CoreLibraryDomain.Default, $"Adding file {fileName} to project.");
            }

            // Add copied file to the project
            string  fileGuid            = project.AddFile(FormatFilePathInProject(destinationFilePath, rooted: false),  parentGroup + fileName);
            project.AddFileToBuildWithFlags(
                targetGuid,
                fileGuid,
                compileFlags.IsNullOrEmpty() ? string.Empty : string.Join(" ", compileFlags));

            // Add search path project
            string  fileExtension       = Path.GetExtension(destinationFilePath);
            if (string.Equals(fileExtension, ".a", StringComparison.InvariantCultureIgnoreCase))
            {
                CacheLibrarySearchPath(destinationFilePath);
            }
            else if (string.Equals(fileExtension, ".framework", StringComparison.InvariantCultureIgnoreCase))
            {
                CacheFrameworkSearchPath(destinationFilePath);
            }
        }

        private void AddFolderToProject(PBXProject project, string sourceFolder, string targetGuid, string parentGroup, string[] compileFlags)
        {
            // Check whether given folder is valid
            var     sourceFolderInfo    = new DirectoryInfo(sourceFolder);
            if (!sourceFolderInfo.Exists) return;

            // Add files placed within this folder
            foreach (var fileInfo in FindFiles(sourceFolderInfo))
            {
                AddFileToProject(
                    project,
                    fileInfo.FullName,
                    targetGuid,
                    parentGroup,
                    compileFlags);
            }

            // add folders placed within this folder
            foreach (var subFolderInfo in sourceFolderInfo.GetDirectories())
            {
                string  folderGroup     = parentGroup + subFolderInfo.Name + "/";
                AddFolderToProject(
                    project,
                    subFolderInfo.FullName,
                    targetGuid,
                    folderGroup,
                    compileFlags);
            }
        }

        private string CopyFileToProject(string filePath, string targetFolder)
        {
#if NATIVE_PLUGINS_DEBUG
            return filePath;
#else
            // create target folder directory, incase if it doesn't exist
            if (!IOServices.DirectoryExists(targetFolder))
            {
                IOServices.CreateDirectory(targetFolder);
            }

            // copy specified file
            string  fileName        = Path.GetFileName(filePath);
            string  destPath        = Path.Combine(targetFolder, fileName);

            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Copying file {filePath} to {destPath}.");
            FileUtil.CopyFileOrDirectory(filePath, destPath);

            return destPath;
#endif
        }

        private string GetFilePathInProject(string sourcePath, string parentFolder, string parentGroup)
        {
#if NATIVE_PLUGINS_DEBUG
            return sourcePath;
#else
            string  relativePath        = IOServices.GetRelativePath(parentFolder, sourcePath);
            string  destinationFolder   = IOServices.CombinePath(OutputPath, parentGroup);
            return IOServices.CombinePath(destinationFolder, relativePath);
#endif
        }

        private string FormatFilePathInProject(string path, bool rooted = true)
        {
#if NATIVE_PLUGINS_DEBUG
            return path;
#else
            if (path.Contains("$(inherited)"))
            {
                return path;
            }

            string  relativePathToProject   = IOServices.GetRelativePath(OutputPath, path);
            return rooted ? Path.Combine("$(SRCROOT)", relativePathToProject) : relativePathToProject;
#endif
        }

        private void CacheLibrarySearchPath(string path)
        {
            string  directoryPath   = Path.GetDirectoryName(path);
            m_librarySearchPaths.AddUnique(directoryPath);
        }

        private void CacheFrameworkSearchPath(string path)
        {
            string  directoryPath   = Path.GetDirectoryName(path);
            m_frameworkSearchPaths.AddUnique(directoryPath);
        }

        private FileInfo[] FindFiles(DirectoryInfo folder)
        {
            return folder.GetFiles().Where((fileInfo) =>
            {
                string  fileExtension   = fileInfo.Extension;
                return !Array.Exists(kIgnoreFileExtensions, (ignoreExt) => string.Equals(fileExtension, ignoreExt, StringComparison.InvariantCultureIgnoreCase));
            }).ToArray();
        }

        // Added for supporting notification services custom sound files
        private void CopyAssetsFromStreamingAssets()
        {
            string  mainTargetGuid  = Project.GetMainTargetGuid();

            // Copy audio files from streaming assets if any to Raw folder
            string  path            = UnityEngine.Application.streamingAssetsPath;
            if (IOServices.DirectoryExists(path))
            {
                var     files               = System.IO.Directory.GetFiles(path);
                string  destinationFolder   = OutputPath;

                var     formats             = new string[]
                {
                    ".mp3",
                    ".wav",
                    ".ogg",
                    ".aiff"
                };
                for (int i=0; i< files.Length; i++)
                {
                    string  extension   = IOServices.GetExtension(files[i]);
                    if (formats.Contains(extension.ToLower()))
                    {
                        string destinationRelativePath = files[i].Replace(path, ".");
                        IOServices.CopyFile(files[i], IOServices.CombinePath(destinationFolder, IOServices.GetFileName(files[i])));
                        DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Coping asset with relativePath: {destinationRelativePath}.");
                        Project.AddFileToBuild(mainTargetGuid, Project.AddFile(destinationRelativePath, destinationRelativePath));
                    }
                }
            }
        }

#endregion

#region Misc methods

        private void UpdateInfoPlistDocument()
        {
            DebugLogger.Log(CoreLibraryDomain.Default, "Updating plist configuration.");

            // Open the file
            string  plistPath   = $"{OutputPath}/Info.plist";
            var     plistDoc    = new PlistDocument();
            plistDoc.ReadFromString(File.ReadAllText(plistPath));

            // Send message to all the NativeProcessors
            PluginsProcessors.ForEach(
                (item) =>
                {
                    item.OnUpdateInfoPlist(plistDoc);
                });

            // Save changes
            plistDoc.WriteToFile(plistPath);
        }

        private void UpdateProjectCapabilities()
        {
            var     capabilityManager   = new ProjectCapabilityManager(
                ProjectFilePath,
                GetEntitlementsPath(),
                Project.GetMainTargetName(),
                Project.GetMainTargetGuid());

            // Add the capabilities specified in the Exporters
            foreach (var exporterObject in ActiveExporterObjects)
            {
                if (!exporterObject.IsEnabled) continue;
                    
                foreach (var capability in exporterObject.Capabilities)
                {
                    switch (capability.Type)
                    {
                        case PBXCapabilityType.GameCenter:
                            capabilityManager.AddGameCenter();
                            break;

                        case PBXCapabilityType.iCloud:
                            capabilityManager.AddiCloud(enableKeyValueStorage: true, enableiCloudDocument: false, enablecloudKit: false, addDefaultContainers: false, customContainers: null);
                            break;

                        case PBXCapabilityType.InAppPurchase:
                            capabilityManager.AddInAppPurchase();
                            break;

                        case PBXCapabilityType.PushNotifications:
                            capabilityManager.AddPushNotifications(Debug.isDebugBuild);
                            capabilityManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
                            break;

                        case PBXCapabilityType.AssociatedDomains:
                            var     associatedDomainsEntitlement    = capability.AssociatedDomainsEntitlement;
                            capabilityManager.AddAssociatedDomains(domains: associatedDomainsEntitlement.Domains);
                            break;

                        default:
                            throw VBException.SwitchCaseNotImplemented(capability.Type);
                    }
                }
            }

            // Send message to all the NativeProcessors
            PluginsProcessors.ForEach(
                (item) =>
                {
                    item.OnUpdateCapabilities(capabilityManager);
                });

            // Save changes
            capabilityManager.WriteToFile();
        }

        private void UpdateMacroDefinitions()
        {
            var     preprocessorDirectivesManager   = new MacroDefinitionsManager(
                path: kPreprocessorFilePath,
                headerComments: "//  NPConfig.h" +
                    "//  Native Plugins" +
                    "//" +
                    "//  Created by Ashwin kumar on 22/01/19." +
                    "//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved." +
                    "//");

            // Add macros from Settings file
            foreach (var exporterObject in ActiveExporterObjects)
            {
                var     macros  = exporterObject.Macros;
                foreach (var entry in macros)
                {
                    preprocessorDirectivesManager.AddMacro($"{entry.Key} {entry.Value}");
                }
            }

            // Send message to add macros to all the Processor instances
            PluginsProcessors.ForEach((item) => item.OnUpdateMacroDefinitions(preprocessorDirectivesManager));

            // Serialize
            preprocessorDirectivesManager.WriteToFile();
        }

        private string GetEntitlementsPath()
		{
			var     mainTargetGuid  = Project.GetMainTargetGuid();
			var     mainTargetName  = Project.GetMainTargetName();

            var     relativePath    = Project.GetBuildPropertyForAnyConfig(mainTargetGuid, BuildConfigurationKey.kCodeSignEntitlements);
			if (relativePath != null)
			{
				var     fullPath    = Path.Combine(OutputPath, relativePath);
				if (IOServices.FileExists(fullPath))
				{
					return fullPath;
				}
			}

            //  Make new file
			var     entitlementsPath    = Path.Combine(OutputPath, mainTargetName, $"{mainTargetName}.entitlements");
			var     entitlementsPlist   = new PlistDocument();
			entitlementsPlist.WriteToFile(entitlementsPath);

			// Copy the entitlement file to the xcode project
			var     entitlementFileName = Path.GetFileName(entitlementsPath);
			var     relativeDestination = $"{mainTargetName}/{entitlementFileName}";

			// Add the pbx configs to include the entitlements files on the project
			Project.AddFile(relativeDestination, entitlementFileName);
			Project.SetBuildProperty(mainTargetGuid, BuildConfigurationKey.kCodeSignEntitlements, relativeDestination);


            //@@ Adding patch to avoid build errors on windows (Need to cross check on windows system. This patch is provided by Baldrekr developer (ref: https://discord.com/channels/672868273779507223/760780555947474944/1097242018255732756))
            // If these are both absolute paths, strip off the common part from entitlements.
            if (entitlementsPath.Contains(':') && ProjectFilePath.Contains(':'))
            {
                int maxStrip = Math.Min(entitlementsPath.Length, ProjectFilePath.Length);
                int i = 0;
                while (i < maxStrip && ProjectFilePath[i] == entitlementsPath[i])
                {
                    i++;
                }
                entitlementsPath = entitlementsPath.Substring(i);
            }

            return entitlementsPath;
		}

        private void ClearPluginsData()
        {
            string  pluginExportPath    = Path.Combine(OutputPath, kPluginRelativePath);
            IOServices.DeleteDirectory(pluginExportPath);
        }

#endregion
    }
}
#endif