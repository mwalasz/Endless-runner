using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public interface INativeAlertDialogInterface : INativeObject
    {   
        #region Events

        event AlertButtonClickInternalCallback OnButtonClick;

        #endregion

        #region Methods

        // setters and getter methods
        void SetTitle(string value);

        string GetTitle();

        void SetMessage(string value);

        string GetMessage();

        // action methods
        void AddButton(string text, bool isCancelType);

        // presentation methods
        void Show();
        
        void Dismiss();

        #endregion
    }
}