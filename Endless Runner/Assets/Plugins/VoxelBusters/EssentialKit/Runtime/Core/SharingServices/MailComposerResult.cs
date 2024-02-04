using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the result of the user action which caused <see cref="MailComposer"/> interface to dismiss.
    /// </summary>
    public class MailComposerResult
    {
        #region Properties

        /// <summary>
        /// Gets the result of the user’s action.
        /// </summary>
        /// <value>The result code of user’s action.</value>
        public MailComposerResultCode ResultCode { get; private set; }

        #endregion

        #region Constructors

        internal MailComposerResult(MailComposerResultCode resultCode)
        {
            // Set properties
            ResultCode  = resultCode;
        }

        #endregion
    }
}