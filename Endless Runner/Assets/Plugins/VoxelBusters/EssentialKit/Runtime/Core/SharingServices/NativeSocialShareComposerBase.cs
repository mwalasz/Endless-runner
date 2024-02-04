using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
	public abstract class NativeSocialShareComposerBase : NativeObjectBase, INativeSocialShareComposer 
	{
		#region INativeSocialShareComposer implementation

        public event SocialShareComposerClosedInternalCallback OnClose;

        public abstract void SetText(string value);
        
		public abstract void AddScreenshot();
        
		public abstract void AddImage(byte[] imageData);

        public abstract void AddURL(URLString url);

        public abstract void Show(Vector2 screenPosition);

		#endregion

        #region Private methods

        protected void SendCloseEvent(SocialShareComposerResultCode resultCode, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnClose(resultCode, error));
        }

        #endregion
	}
}