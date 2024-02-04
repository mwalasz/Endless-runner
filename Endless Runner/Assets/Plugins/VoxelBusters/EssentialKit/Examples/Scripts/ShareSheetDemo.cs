using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public class ShareSheetDemo : DemoActionPanelBase<ShareSheetDemoAction, ShareSheetDemoActionType>
	{
        #region Fields

        [SerializeField]
        private     RectTransform[]     m_instanceDependentObjects      = null;

        [SerializeField]
        private     InputField          m_textInputField                = null;

        [SerializeField]
        private     InputField          m_urlInputField                 = null;

        [SerializeField]
        private     Toggle              m_isLocalPathToggle             = null;

        private     ShareSheet          m_shareSheet	                = null;

        #endregion
        
        #region Base class methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            SetActiveSheet(null);
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "ShareSheet.CreateInstance()";
        }

        protected override void OnActionSelectInternal(ShareSheetDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case ShareSheetDemoActionType.New:
                    Log("Creating new share sheet.");
                    // create instance
                    var     newSheet    = ShareSheet.CreateInstance();
                    newSheet.SetCompletionCallback(OnShareSheetClosed);

                    // update UI
                    SetActiveSheet(newSheet);
                    break;

                case ShareSheetDemoActionType.AddText:
                    Log("Adding text.");
                    m_shareSheet.AddText(m_textInputField.text);
                    break;

                case ShareSheetDemoActionType.AddScreenshot:
                    Log("Adding screenshot.");
                    m_shareSheet.AddScreenshot();
                    break;

                case ShareSheetDemoActionType.AddImage:
                    Log("Adding random image.");
                    var     image   = DemoResources.GetRandomImage();
                    m_shareSheet.AddImage(image);
                    break;

                case ShareSheetDemoActionType.AddURL:
                    Log("Adding url.");
                    var     url     = m_isLocalPathToggle.isOn 
                        ? URLString.FileURLWithPath(m_urlInputField.text) 
                        : URLString.URLWithPath(m_urlInputField.text);
                    m_shareSheet.AddURL(url);
					break;

                case ShareSheetDemoActionType.Show:
                    Log("Showing share sheet now.");
                    m_shareSheet.Show();
                    break;

                case ShareSheetDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kSharingServices);
                    break;

                default:
                    break;
            }
        }

        private void OnShareSheetClosed(ShareSheetResult result, Error error)
        {
            Log("Share sheet was closed. Result code: " + result.ResultCode);

            // reset instance
            SetActiveSheet(null);
        }

        #endregion

        #region Private methods

        private void SetActiveSheet(ShareSheet sheet)
        {
            // set property
            m_shareSheet    = sheet;

            // update ui
            SetInstanceDependentObjectState(sheet != null);
        }

        private void SetInstanceDependentObjectState(bool active)
        {
            foreach (RectTransform rect in m_instanceDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }
        }

        #endregion
	}
}
