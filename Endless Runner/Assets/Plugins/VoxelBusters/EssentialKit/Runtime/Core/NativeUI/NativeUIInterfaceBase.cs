using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NativeUICore
{
	public abstract class NativeUIInterfaceBase : NativeFeatureInterfaceBase, INativeUIInterface 
	{
        #region Private methods

        protected NativeUIInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeUIInterface implementation

        public abstract INativeAlertDialogInterface CreateAlertDialog(AlertDialogStyle style);
		
        public abstract INativeDatePickerInterface CreateDatePicker(DatePickerMode mode);

        #endregion
	}
}