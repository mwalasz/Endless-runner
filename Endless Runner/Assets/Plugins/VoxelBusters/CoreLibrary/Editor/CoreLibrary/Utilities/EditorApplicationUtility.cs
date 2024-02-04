using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class EditorApplicationUtility
    {
        #region Platform methods

        public static RuntimePlatform ConvertBuildTargetToRuntimePlatform(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.iOS:
                    return RuntimePlatform.IPhonePlayer;

                case BuildTarget.tvOS:
                    return RuntimePlatform.tvOS;

                case BuildTarget.Android:
                    return RuntimePlatform.Android;

                default:
                    return (RuntimePlatform)(-1);
            }
        }

        #endregion
    }
}