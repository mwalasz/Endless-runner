using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// An enumeration for the available alert dialog styles.
    /// </summary>
    public enum AlertDialogStyle 
    {
        /// <summary> An overlay alert dialog is displayed. </summary>
        Default,

        /// <summary> An action sheet style is used to display alert. (iOS feature) </summary>
        ActionSheet,
    }
}