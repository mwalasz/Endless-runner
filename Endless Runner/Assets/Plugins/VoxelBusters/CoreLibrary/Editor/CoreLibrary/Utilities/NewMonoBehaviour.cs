using UnityEngine;
using System.Collections;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class DeleteAssetRequest : AsyncOperation<bool>
    {
        #region Properties

        public string Path { get; private set; }

        #endregion

        #region Constructors

        public DeleteAssetRequest(string path)
        {
            Path     = path;
        }

        #endregion

        #region Base class methods

        protected override void OnStart()
        {
            if (!AssetDatabase.DeleteAsset(Path))
            {
                SetIsCompleted(new Error("File not found."));
                return;
            }
            SetIsCompleted(true);
        }

        #endregion

    }
}