#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;
namespace VoxelBusters.EssentialKit.AddressBookCore.Android
{
    public class NativeAddressBook : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion

        #region Constructor

        public NativeAddressBook(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
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

        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAddressBook][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public bool IsAuthorized()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAddressBook][Method : IsAuthorized]");
#endif
            return Call<bool>(Native.Method.kIsAuthorized);
        }
        public void ReadContacts(NativeReadContactsListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAddressBook][Method : ReadContacts]");
#endif
            Call(Native.Method.kReadContacts, new object[] { listener } );
        }
        public void RequestPermission(NativeRequestContactsPermissionListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAddressBook][Method : RequestPermission]");
#endif
            Call(Native.Method.kRequestPermission, new object[] { listener } );
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.addressbook.AddressBook";

            internal class Method
            {
                internal const string kReadContacts = "readContacts";
                internal const string kIsAuthorized = "isAuthorized";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kRequestPermission = "requestPermission";
            }

        }
    }
}
#endif