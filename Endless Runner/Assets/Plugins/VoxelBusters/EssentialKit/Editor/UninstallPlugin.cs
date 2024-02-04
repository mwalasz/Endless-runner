using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.Editor
{
	public class UninstallPlugin
	{
		#region Constants
	
		private const	string		kAlertTitle		= "Uninstall - Cross Platform Native Plugin : Essential Kit";
        
		private const	string		kAlertMessage	= "Backup before doing this step to preserve changes done in this plugin. This deletes files only related to Cross Platform Native Plugins - Essential Kit plugin. Do you want to proceed?";
		
		#endregion	
	
		#region Static methods
	
		public static void Uninstall()
		{
			bool	confirmationStatus	= EditorUtility.DisplayDialog(kAlertTitle, kAlertMessage, "Uninstall", "Cancel");
			if (!confirmationStatus) return;

			// delete all the files and folders belonging to the plugin
			var		pluginFolders		= new string[]
			{
				EssentialKitSettings.Package.GetInstallPath(),
				//EssentialKitSettings.DefaultSettingsAssetPath,
				EssentialKitPackageLayout.ExtrasPath
			};
			foreach (string folder in pluginFolders)
			{
                string	absolutePath	= Application.dataPath + "/../" + folder;
                IOServices.DeleteFileOrDirectory(absolutePath);
                IOServices.DeleteFileOrDirectory(absolutePath + ".meta");
			}
			EssentialKitSettingsEditorUtility.RemoveGlobalDefines();
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Cross Platform Native Plugins : Essential Kit",
				                        "Uninstall successful!", 
				                        "Ok");
		}
		
		#endregion
	}
}