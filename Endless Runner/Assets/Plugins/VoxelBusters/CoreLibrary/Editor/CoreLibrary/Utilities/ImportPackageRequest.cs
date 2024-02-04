using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class ImportPackageRequest : VoxelBusters.CoreLibrary.AsyncOperation<bool>
    {
        #region Properties

        public string Path { get; private set; }

        #endregion

        #region Constructors

        public ImportPackageRequest(string path)
        {
            Path     = path;
        }

        #endregion

        #region Base class methods

        protected override void OnStart()
        {
            if (!IOServices.FileExists(Path))
            {
                SetIsCompleted(new Error("File not found."));
                return;
            }

            AssetDatabase.ImportPackage(Path, interactive: false);
            SetIsCompleted(true);
        }

        #endregion
    }
}