#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public class NativeShareSheet : NativeAndroidJavaObjectWrapper
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

        public NativeShareSheet(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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
        #endregion
        #region Public methods

        public void AddAttachment(NativeBytesWrapper data, string mimeType, string fileName)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeShareSheet][Method : AddAttachment]");
#endif
            Call(Native.Method.kAddAttachment, new object[] { data.NativeObject, mimeType, fileName } );
        }
        public void AddAttachmentAsync(string path, string mimeType)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeShareSheet][Method : AddAttachmentAsync]");
#endif
            Call(Native.Method.kAddAttachmentAsync, new object[] { path, mimeType } );
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeShareSheet][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public string GetSaveDirectory()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeShareSheet][Method : GetSaveDirectory]");
#endif
            return Call<string>(Native.Method.kGetSaveDirectory);
        }
        public void SetText(string value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeShareSheet][Method : SetText]");
#endif
            Call(Native.Method.kSetText, new object[] { value } );
        }
        public void SetUrl(string urlString)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeShareSheet][Method : SetUrl]");
#endif
            Call(Native.Method.kSetUrl, new object[] { urlString } );
        }
        public void Show(NativeShareSheetListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeShareSheet][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, new object[] { listener } );
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.ShareSheet";

            internal class Method
            {
                internal const string kAddAttachment = "addAttachment";
                internal const string kSetText = "setText";
                internal const string kAddAttachmentAsync = "addAttachmentAsync";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kSetUrl = "setUrl";
                internal const string kGetSaveDirectory = "getSaveDirectory";
                internal const string kShow = "show";
            }

        }
    }
}
#endif