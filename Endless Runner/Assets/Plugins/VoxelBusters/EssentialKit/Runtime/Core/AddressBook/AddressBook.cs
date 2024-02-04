using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.AddressBookCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The AddressBook class provides cross-platform interface to access the contact information.
    /// </summary>
    /// <description> 
    /// <para>
    /// In iOS platform, users can grant or deny access to contact data on a per-application basis. 
    /// And the user is prompted only the first time <see cref="ReadContacts(EventCallback{AddressBookReadContactsResult})"/> is requested; any subsequent calls use the existing permissions.
    /// You can provide custom usage description in Address Book settings of NativePluginsSettings window.
    /// </para>
    /// </description> 
    public static class AddressBook
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativeAddressBookInterface    s_nativeInterface    = null;

        #endregion

        #region Static properties

        public static AddressBookUnitySettings UnitySettings { get; private set; }

        #endregion

        #region Static methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        public static void Initialize(AddressBookUnitySettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // Set default properties
            UnitySettings                       = settings;
            AddressBookContactBase.defaultImage = settings.DefaultImage;

            // Configure interface
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeAddressBookInterface>(ImplementationSchema.AddressBook, settings.IsEnabled);
        }

        /// <summary>
        /// Returns the current permission status provided to access the contact data.
        /// </summary>
        /// <description>
        /// To see different authorization status, see <see cref="AddressBookContactsAccessStatus"/>.
        /// </description>
        /// <returns>The current permission status to access the contact data.</returns>
        public static AddressBookContactsAccessStatus GetContactsAccessStatus()
        {
            try
            {
                // make request
                return s_nativeInterface.GetContactsAccessStatus();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
                return AddressBookContactsAccessStatus.NotDetermined;
            }
        }

        /// <summary>
        /// Requests permission to access contact data.
        /// </summary>
        /// <param name="showPrepermissionDialog">Indicates whether pre-confirmation is required, before prompting system permission dialog.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void RequestContactsAccess(bool showPrepermissionDialog = true, EventCallback<AddressBookRequestContactsAccessResult> callback = null)
        {
            var     permissionHandler   = NativeFeatureUsagePermissionHandler.Default;
            if (showPrepermissionDialog && (permissionHandler != null))
            {
                permissionHandler.ShowPrepermissionDialog(
                    permissionType: NativeFeatureUsagePermissionType.kAddressBook,
                    onAllowCallback: () =>
                    {
                        // ask user permission
                        RequestContactsAccessInternal(callback);
                    },
                    onDenyCallback: () =>
                    {
                        CallbackDispatcher.InvokeOnMainThread(callback, result: null, error: new Error(description: "User cancelled."));
                    });
            }
            else
            {
                RequestContactsAccessInternal(callback);
            }
        }

        /// <summary>
        /// Retrieves all the contact information saved in address book database.
        /// </summary>
        /// <param name="callback">The callback that will be executed after the operation is completed.</param>
        /// <example>
        public static void ReadContacts(EventCallback<AddressBookReadContactsResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.ReadContacts((contacts, error) => SendReadContactsResult(callback, contacts, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Extension methods

        /// <summary>
        /// Retrieves all the contact information saved in address book database. This action checks user permission level and requests for permission if not provided before.
        /// </summary>
        /// <param name="callback">The callback that will be executed after the operation is completed.</param>
        /// <example>
        /// The following code example demonstrates how to read contacts information.
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        /// using VoxelBusters.EssentialKit;
        /// 
        /// public class ExampleClass : MonoBehaviour 
        /// {       
        ///     public void Start()
        ///     {
        ///         // initiate request to read contacts data
        ///         AddressBook.ReadContactsWithUserPermission(OnReadContactsFinished); 
        ///     }
        /// 
        ///     // callback method executed when read request is finished
        ///     private void OnReadContactsFinished(AddressBookReadContactsResult data, NativeResultException exception)
        ///     {
        ///         if (null == exception)
        ///         {
        ///             IAddressBookContact[]   contacts    = data.Contacts;
        ///             foreach (IAddressBookContact entry in contacts)
        ///             {
        ///                 Debug.Log(entry);
        ///             }
        ///         }
        ///         else
        ///         {
        ///             // user didn't provide necessary permission
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void ReadContactsWithUserPermission(EventCallback<AddressBookReadContactsResult> callback)
        {
            try
            {
                // get user permission incase if its not provided yet
                var     accessStatus    = GetContactsAccessStatus();
                if (accessStatus == AddressBookContactsAccessStatus.NotDetermined)
                {
                    RequestContactsAccess(showPrepermissionDialog: true, callback: (result, error) =>
                    {
                        ReadContactsInSafeMode(result.AccessStatus, callback);
                    });
                }
                else
                {
                    ReadContactsInSafeMode(accessStatus, callback);
                }
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        #endregion

        #region Private static methods

        private static void RequestContactsAccessInternal(EventCallback<AddressBookRequestContactsAccessResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.RequestContactsAccess((status, error) =>
                {
                    // send result to caller object
                    var     result      = new AddressBookRequestContactsAccessResult(accessStatus: status);
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(EssentialKitDomain.Default, exception);
            }
        }

        private static void ReadContactsInSafeMode(AddressBookContactsAccessStatus accessStatus, EventCallback<AddressBookReadContactsResult> callback)
        {
            if (AddressBookContactsAccessStatus.Authorized == accessStatus)
            {
                // make request
                ReadContacts(callback);
            }
            else
            {
                SendReadContactsResult(callback, null, new Error(description: "Not authorized to access contacts database."));
            }
        }

        #endregion

        #region Callback methods

        private static void SendReadContactsResult(EventCallback<AddressBookReadContactsResult> callback, IAddressBookContact[] contacts, Error error)
        {
            // send result to caller object
            var     result  = new AddressBookReadContactsResult(contacts: contacts ?? new IAddressBookContact[0]);
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        #endregion
    }
}