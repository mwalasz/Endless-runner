using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Result codes returned when the <see cref="ShareSheet"/> interface is dismissed.
    /// </summary>
    public enum ShareSheetResultCode
    {
        /// <summary> The user action could not be determined.  This occurs in platforms where there is no provision to find result. </summary>
        Unknown,

        /// <summary> The user cancelled the operation. </summary>
        Cancelled,

        /// <summary> The user has completed action by selecting one of the service. </summary>
        Done,
    }
}