using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum ShareSheetDemoActionType
	{
		New,
		AddText,
		AddScreenshot,
		AddImage,
		AddURL,
		Show,
		ResourcePage
	}

	public class ShareSheetDemoAction : DemoActionBehaviour<ShareSheetDemoActionType> 
	{}
}
