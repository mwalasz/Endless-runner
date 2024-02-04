using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public enum SocialShareComposerDemoActionType
	{
		IsComposerAvailable,
		New,
		SetText,
		AddScreenshot,
		AddImage,
		AddURL,
		Show,
		ResourcePage
	}

	public class SocialShareComposerDemoAction : DemoActionBehaviour<SocialShareComposerDemoActionType> 
	{}
}