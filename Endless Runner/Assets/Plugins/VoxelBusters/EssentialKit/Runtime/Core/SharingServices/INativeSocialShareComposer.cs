using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public interface INativeSocialShareComposer : INativeObject
    {
        #region Events

        event SocialShareComposerClosedInternalCallback OnClose;

        #endregion

        #region Methods
        
        // attachment options
        void SetText(string value);

        void AddScreenshot();
        
        void AddImage(byte[] imageData);
        
        void AddURL(URLString url);

        // presentation methods
        void Show(Vector2 screenPosition);
        
        #endregion
    }
}