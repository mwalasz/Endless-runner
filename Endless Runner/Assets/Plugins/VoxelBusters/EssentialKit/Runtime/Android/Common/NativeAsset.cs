#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.Common.Android
{
    public class NativeAsset : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeAsset(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeAsset(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeAsset()
        {
            DebugLogger.Log("Disposing NativeAsset");
        }
#endif
        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }

        #endregion
        #region Public methods

        public bool IsValid()
        {
            return Call<bool>(Native.Method.kIsValid);
        }
        public void Load(NativeLoadAssetListener listener)
        {
            Call(Native.Method.kLoad, listener);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.utilities.common.Asset";

            internal class Method
            {
                internal const string kIsValid = "isValid";
                internal const string kLoad = "load";
            }

        }
    }
}
#endif