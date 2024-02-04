using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore
{
	public abstract class NativeAlertDialogInterfaceBase : NativeObjectBase, INativeAlertDialogInterface
	{
        #region INativeAlertDialog implementation

        public event AlertButtonClickInternalCallback OnButtonClick;

		public abstract void SetTitle(string value);

        public abstract string GetTitle();

        public abstract void SetMessage(string value);

        public abstract string GetMessage();

        public abstract void AddButton(string text, bool isCancelType);

        public abstract void Show();
		
        public abstract void Dismiss();

		#endregion

        #region Private methods

        protected void SendButtonClickEvent(int selectedButtonIndex)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnButtonClick(selectedButtonIndex));
        }

        #endregion
	}
}