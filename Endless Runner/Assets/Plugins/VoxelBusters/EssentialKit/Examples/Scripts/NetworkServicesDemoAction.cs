using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum NetworkServicesDemoActionType
	{
		IsInternetActive,
		IsHostReachable,
		StartNotifier,
		StopNotifier,
        ResourcePage,
        IsNotifierActive,
	}

	public class NetworkServicesDemoAction : DemoActionBehaviour<NetworkServicesDemoActionType> 
	{}
}