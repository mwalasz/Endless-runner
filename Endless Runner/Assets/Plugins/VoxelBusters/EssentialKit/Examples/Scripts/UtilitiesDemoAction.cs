using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum UtilitiesDemoActionType
	{
		OpenAppStorePage,
        OpenCustomAppStorePage,
		ResourcePage,
        OpenApplicationSettings,
	}

	public class UtilitiesDemoAction : DemoActionBehaviour<UtilitiesDemoActionType> 
	{}
}