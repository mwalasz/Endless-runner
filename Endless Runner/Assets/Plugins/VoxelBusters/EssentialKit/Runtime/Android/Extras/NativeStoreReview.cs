#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.ExtrasCore.Android
{
    public class NativeStoreReview : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Private properties
        private NativeActivity Activity
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        public NativeStoreReview(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
            Activity    = new NativeActivity(context);
        }

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
        public static bool CanRequestStoreReview()
        {
            return GetClass().CallStatic<bool>(Native.Method.kCanRequestStoreReview);
        }

        #endregion
        #region Public methods

        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeStoreReview][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void RequestStoreReview(NativeRequestStoreReviewListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeStoreReview][Method(RunOnUiThread) : RequestStoreReview]");
#endif
                Call(Native.Method.kRequestStoreReview, new object[] { listener } );
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.extras.StoreReview";

            internal class Method
            {
                internal const string kRequestStoreReview = "requestStoreReview";
                internal const string kCanRequestStoreReview = "canRequestStoreReview";
                internal const string kGetFeatureName = "getFeatureName";
            }

        }
    }
}
#endif