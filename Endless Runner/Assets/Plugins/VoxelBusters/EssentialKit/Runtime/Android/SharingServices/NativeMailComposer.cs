#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public class NativeMailComposer : NativeAndroidJavaObjectWrapper
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

        public NativeMailComposer(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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
        public static bool CanSendMail(NativeContext context)
        {
            return GetClass().CallStatic<bool>(Native.Method.kCanSendMail, new object[] { context.NativeObject } );
        }

        #endregion
        #region Public methods

        public void AddAttachment(NativeBytesWrapper data, string mimeType, string fileName)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : AddAttachment]");
#endif
            Call(Native.Method.kAddAttachment, new object[] { data.NativeObject, mimeType, fileName } );
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void SetBccRecipients(string[] recipients)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : SetBccRecipients]");
#endif
            Call(Native.Method.kSetBccRecipients, new object[] { recipients } );
        }
        public void SetBody(string body, bool isHtml)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : SetBody]");
#endif
            Call(Native.Method.kSetBody, new object[] { body, isHtml } );
        }
        public void SetCcRecipients(string[] recipients)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : SetCcRecipients]");
#endif
            Call(Native.Method.kSetCcRecipients, new object[] { recipients } );
        }
        public void SetSubject(string subject)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : SetSubject]");
#endif
            Call(Native.Method.kSetSubject, new object[] { subject } );
        }
        public void SetToRecipients(string[] recipients)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMailComposer][Method : SetToRecipients]");
#endif
            Call(Native.Method.kSetToRecipients, new object[] { recipients } );
        }
        public void Show(NativeMailComposerListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMailComposer][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, new object[] { listener } );
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.sharingservices.MailComposer";

            internal class Method
            {
                internal const string kAddAttachment = "addAttachment";
                internal const string kSetBody = "setBody";
                internal const string kCanSendMail = "canSendMail";
                internal const string kSetSubject = "setSubject";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kSetToRecipients = "setToRecipients";
                internal const string kSetCcRecipients = "setCcRecipients";
                internal const string kSetBccRecipients = "setBccRecipients";
                internal const string kShow = "show";
            }

        }
    }
}
#endif