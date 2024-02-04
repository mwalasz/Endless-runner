#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using VoxelBusters.EssentialKit.Common.Android;

namespace VoxelBusters.EssentialKit.AddressBookCore.Android
{
    public class NativeAddressBookContact : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeAddressBookContact(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeAddressBookContact(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeAddressBookContact()
        {
            DebugLogger.Log("Disposing NativeAddressBookContact");
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

        public string GetDisplayName()
        {
            return Call<string>(Native.Method.kGetDisplayName);
        }
        public string[] GetEmailAddresses()
        {
            return Call<string[]>(Native.Method.kGetEmailAddresses);
        }
        public string GetFamilyName()
        {
            return Call<string>(Native.Method.kGetFamilyName);
        }
        public string GetGivenName()
        {
            return Call<string>(Native.Method.kGetGivenName);
        }
        public string[] GetPhoneNumbers()
        {
            return Call<string[]>(Native.Method.kGetPhoneNumbers);
        }
        public NativeAsset GetProfilePicture()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetProfilePicture);
            NativeAsset data  = new  NativeAsset(nativeObj);
            return data;
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.essentialkit.addressbook.AddressBookContact";

            internal class Method
            {
                internal const string kGetGivenName = "getGivenName";
                internal const string kGetFamilyName = "getFamilyName";
                internal const string kGetDisplayName = "getDisplayName";
                internal const string kGetPhoneNumbers = "getPhoneNumbers";
                internal const string kGetEmailAddresses = "getEmailAddresses";
                internal const string kGetProfilePicture = "getProfilePicture";
            }

        }
    }
}
#endif