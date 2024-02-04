using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [System.Serializable]
    public class UnityPackageDefinition
    {
        #region Fields

        private     string      m_persistentDataRelativePath;

        #endregion

        #region Properties

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public string Version { get; private set; }

        public string DefaultInstallPath { get; private set; }

        public string UpmInstallPath { get; private set; }

        public string MutableResourcesPath { get; private set; }

        public string MutableResourcesRelativePath { get; private set; }

        public string PersistentDataRelativePath
        {
            get
            {
                EnsurePersistentDataPathExists();
                return m_persistentDataRelativePath;
            }
        }

        public string PersistentDataPath
        {
            get
            {
                EnsurePersistentDataPathExists();
                return GetPersistentDataPathInternal();
            }
        }

        public UnityPackageDefinition[] Dependencies { get; private set; }

        #endregion

        #region Constructors

        public UnityPackageDefinition(string name, string displayName,
            string version, string defaultInstallPath = null,
            string mutableResourcesPath = "Assets/Resources", string persistentDataRelativePath = null,
            params UnityPackageDefinition[] dependencies)
        {
            // Set properties
            Name                            = name;
            DisplayName                     = displayName;
            Version                         = version;
            DefaultInstallPath              = defaultInstallPath ?? $"Assets/{Name}";
            UpmInstallPath                  = $"Packages/{Name}";
            MutableResourcesPath            = mutableResourcesPath;
            Dependencies                    = dependencies;

            // Derived properties
            MutableResourcesRelativePath    = mutableResourcesPath.Replace("Assets/Resources", "").TrimStart('/');
            m_persistentDataRelativePath    = persistentDataRelativePath ?? $"VoxelBusters/{string.Join("", displayName.Split(' '))}";
        }

        #endregion

        #region Private methods

        private void EnsurePersistentDataPathExists()
        {
            var     fullPath    = GetPersistentDataPathInternal();
            IOServices.CreateDirectory(fullPath, overwrite: false);
        }

        private string GetPersistentDataPathInternal()
        {
            return IOServices.CombinePath(Application.persistentDataPath, m_persistentDataRelativePath);
        }

        #endregion
    }
}