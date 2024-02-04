using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides an interface to create a custom component to control rating request behaviour.
    /// </summary>
    public interface IRateMyAppController
    {
        #region Methods

        /// <summary>
        /// Returns a boolean value indicating whether rating window can be shown or not.
        /// </summary>
        /// <returns><c>true</c>, if show rate my app can be displayed, <c>false</c> otherwise.</returns>
        bool CanShowRateMyApp();

        /// <summary>
        /// Callback received when user clicks on remind later button.
        /// </summary>
        void DidClickOnRemindLaterButton();

        /// <summary>
        /// Callback received when user clicks on cancel button.
        /// </summary>
        void DidClickOnCancelButton();

        /// <summary>
        /// Callback received when user clicks on ok button.
        /// </summary>
        void DidClickOnOkButton();
        
        #endregion
    }
}