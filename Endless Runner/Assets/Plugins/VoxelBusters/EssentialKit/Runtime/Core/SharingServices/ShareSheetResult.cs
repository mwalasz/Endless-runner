using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the result of the user action which caused <see cref="ShareSheet"/> interface to dismiss.
    /// </summary>
    public class ShareSheetResult
    {
        #region Properties

        /// <summary>
        /// Gets the result of the user’s action.
        /// </summary>
        /// <value>The result code of user’s action.</value>
        public ShareSheetResultCode ResultCode { get; private set; }

        #endregion

        #region Constructors

        internal ShareSheetResult(ShareSheetResultCode resultCode)
        {
            // Set properties
            ResultCode  = resultCode;
        }

        #endregion
    }
}