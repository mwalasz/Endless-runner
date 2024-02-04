using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum DatePickerDemoActionType
	{
		New,
		Show,
		ResourcePage,
	}

	public class DatePickerDemoAction : DemoActionBehaviour<DatePickerDemoActionType> 
	{}
}