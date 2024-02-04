using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the result of the user action which caused <see cref="MessageComposer"/> interface to dismiss.
    /// </summary>
    public class MessageComposerResult
    {
        #region Properties

        /// <summary>
        /// Gets the result code.
        /// </summary>
        /// <value>The result code of user’s action.</value>
        public MessageComposerResultCode ResultCode { get; private set; }

        #endregion

        #region Constructors

        internal MessageComposerResult(MessageComposerResultCode resultCode)
        {
            // Set properties
            ResultCode  = resultCode;
        }

        #endregion
    }
}