using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Possible values for the result, when <see cref="SocialShareComposer"/> interface is dismissed.
    /// </summary>
    public enum SocialShareComposerResultCode
    {
        /// <summary> The composer view is dismissed, but system couldn't determine the result. This occurs in platforms where there is no provision to find result. </summary>
        Unknown,

        /// <summary> The view controller is dismissed without sending the post. For example, the user selects Cancel or the account is not available. </summary>
        Cancelled,

        /// <summary> The composer view is dismissed and the message is being sent in the background. This occurs when the user selects Done. </summary>
        Done,
    }
}