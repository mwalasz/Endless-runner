using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Result codes returned when the <see cref="MailComposer"/> interface is dismissed.
    /// </summary>
    public enum MailComposerResultCode
    {
        /// <summary> The user action could not be determined. </summary>
        Unknown,

        /// <summary> The user cancelled the operation. No email message was queued. </summary>
        Cancelled,

        /// <summary> The email message was saved in the user’s Drafts folder. </summary>
        Saved,

        /// <summary> The email message was queued in the user’s outbox. </summary>
        Sent,

        /// <summary> The email message was not saved or queued, possibly due to an error. </summary>
        Failed,
    }
}