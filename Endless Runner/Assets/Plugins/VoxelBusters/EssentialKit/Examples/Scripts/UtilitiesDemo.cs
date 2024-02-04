using System.Text;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class UtilitiesDemo : DemoActionPanelBase<UtilitiesDemoAction, UtilitiesDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     InputField          m_idInputField      = null;

        #endregion

        #region Base class methods

        protected override void OnActionSelectInternal(UtilitiesDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case UtilitiesDemoActionType.OpenAppStorePage:
                    Log("Opening app store page");
                    Utilities.OpenAppStorePage();
                    break;

                case UtilitiesDemoActionType.OpenCustomAppStorePage:
                    string  appId      = m_idInputField.text;
                    if (string.IsNullOrEmpty(appId))
                    {
                        Log("Provide application id.");
                        return;
                    }
                    Log("Opening app store page");
                    Utilities.OpenAppStorePage(appId);
                    break;

                case UtilitiesDemoActionType.OpenApplicationSettings:
                    Utilities.OpenApplicationSettings();
                    break;

                case UtilitiesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kExtras);
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}
