using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    public abstract class AddressBookContactBase : NativeObjectBase, IAddressBookContact
    {
        #region Static fields

        internal    static  Texture2D       defaultImage;

        #endregion

        #region Fields

        private     TextureData             m_cachedData;

        #endregion

        #region Abstract methods

        protected abstract string GetFirstNameInternal();

        protected abstract string GetMiddleNameInternal();

        protected abstract string GetLastNameInternal();

        protected abstract string[] GetPhoneNumbersInternal();

        protected abstract string[] GetEmailAddressesInternal();

        protected abstract void LoadImageInternal(LoadImageInternalCallback callback);

        #endregion

        #region Base methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("AddressBookContact { ");
            sb.Append("FirstName: ").Append(FirstName).Append(" ");
            sb.Append("LastName: ").Append(LastName);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IAddressBookContact implementation

        public string FirstName => GetFirstNameInternal();

        public string MiddleName => GetMiddleNameInternal();

        public string LastName => GetLastNameInternal();

        public string[] PhoneNumbers => GetPhoneNumbersInternal();

        public string[] EmailAddresses => GetEmailAddressesInternal();

        public void LoadImage(EventCallback<TextureData> callback)
        {

            // send the default image if exists
            TextureData proxyData = null;
            if (defaultImage != null)
            {
                proxyData = new TextureData(defaultImage);
                CallbackDispatcher.InvokeOnMainThread(callback, proxyData, null);
            }

            // check whether cached inforamtion is available
            if (null == m_cachedData)
            {
                // make actual call
                LoadImageInternal((rawData, error) =>
                {
                    // create data container
                    var     result      = (rawData == null) ? null : new TextureData(rawData);

                    // load placeholder if no rawData
                    if (result == null)
                    {
                        result = proxyData;
                    }

                    // save result
                    if (result != null)
                    {
                        m_cachedData    = result;
                    }

                    // send result to caller object
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
            }
            else
            {
                CallbackDispatcher.InvokeOnMainThread(callback, m_cachedData, null);
            }
        }

        #endregion
    }
}