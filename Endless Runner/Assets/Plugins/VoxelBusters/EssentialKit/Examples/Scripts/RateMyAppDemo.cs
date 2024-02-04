using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public class RateMyAppDemo : DemoActionPanelBase<RateMyAppDemoAction, RateMyAppDemoActionType>
	{
		#region Base methods

        protected override void OnActionSelectInternal(RateMyAppDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case RateMyAppDemoActionType.AskForReviewNow:
					Log("Asking for review."); 
                    RateMyApp.AskForReviewNow();
                    break;

                case RateMyAppDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kRateMyApp);
                    break;

                default:
                    break;
            }
        }

        #endregion
	}
}