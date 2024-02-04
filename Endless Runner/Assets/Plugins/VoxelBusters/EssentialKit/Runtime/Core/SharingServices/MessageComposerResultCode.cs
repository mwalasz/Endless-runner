using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Result codes returned when the <see cref="MessageComposer"/> interface is dismissed.
    /// </summary>
    public enum MessageComposerResultCode
    {
        /// <summary> The user action could not be determined.  This occurs in platforms where there is no provision to find result. </summary>
        Unknown,

        /// <summary> The user canceled the composition. </summary>
        Cancelled,

        /// <summary> The user successfully queued or sent the message. </summary>
        Sent,

        /// <summary> The message was not saved or queued, possibly due to an error. </summary>
        Failed,
    }
}