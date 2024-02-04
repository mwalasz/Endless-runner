using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.ExtrasCore
{
    public class NullNativeUtilityInterface : NativeUtilityInterfaceBase
    {
        #region Constructors

        public NullNativeUtilityInterface()
            : base(isAvailable: false)
        { }

        #endregion

        #region Base methods

        public override void RequestStoreReview()
        {
            var     appSettings     = EssentialKitSettings.Instance.ApplicationSettings;
            string  appId           = appSettings.GetAppStoreIdForActivePlatform();
            OpenAppStorePage(appId);
        }

        public override void OpenAppStorePage(string applicationId)
        {
            var     activePlatform  = PlatformMappingServices.GetActivePlatform();
            switch (activePlatform)
            {
                case NativePlatform.Android:
                    Application.OpenURL("https://play.google.com/store/apps/details?id=" + applicationId); 
                    break;

                case NativePlatform.iOS:
                case NativePlatform.tvOS:
                    Application.OpenURL("https://itunes.apple.com/app/id" + applicationId);
                    break;
            }
        }

        public override void OpenApplicationSettings()
        {
            Diagnostics.LogNotSupportedInEditor();
        }

        #endregion
    }
}