#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public class NativeSocialShareComposer : NativeAndroidJavaObjectWrapper
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

        public NativeSocialShareComposer(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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
        public static bool IsComposerAvailable(NativeContext context, NativeSocialShareComposerType type)
        {
            return GetClass().CallStatic<bool>(Native.Method.kIsComposerAvailable, new object[] { context.NativeObject, NativeSocialShareComposerTypeHelper.CreateWithValue(type) } );
        }

        #endregion
        #region Public methods

        public void AddAttachment(NativeBytesWrapper data, string mimeType, string fileName)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : AddAttachment]");
#endif
            Call(Native.Method.kAddAttachment, new object[] { data.NativeObject, mimeType, fileName } );
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void SetComposerType(NativeSocialShareComposerType type)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : SetComposerType]");
#endif
            Call(Native.Method.kSetComposerType, new object[] { NativeSocialShareComposerTypeHelper.CreateWithValue(type) } );
        }
        public void SetText(string text)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : SetText]");
#endif
            Call(Native.Method.kSetText, new object[] { text } );
        }
        public void SetUrl(string urlString)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : SetUrl]");
#endif
            Call(Native.Method.kSetUrl, new object[] { urlString } );
        }
        public void Show(NativeSocialShareComposerListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeSocialShareComposer][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, new object[] { listener } );
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.SocialShareComposer";

            internal class Method
            {
                internal const string kAddAttachment = "addAttachment";
                internal const string kIsComposerAvailable = "isComposerAvailable";
                internal const string kSetText = "setText";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kSetComposerType = "setComposerType";
                internal const string kSetUrl = "setUrl";
                internal const string kShow = "show";
            }

        }
    }
}
#endif