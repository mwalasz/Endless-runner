using System.Collections;
using System.Collections.Generic;
using System;
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
	public class SocialShareComposerDemo : DemoActionPanelBase<SocialShareComposerDemoAction, SocialShareComposerDemoActionType>
	{
        #region Fields

        [SerializeField]
        private     RectTransform[]         m_instanceDependentObjects      = null;

        [SerializeField]
        private     Dropdown                m_typeDropdown                  = null;

        [SerializeField]
        private     InputField              m_textInputField                = null;

        [SerializeField]
        private     InputField              m_urlInputField                 = null;

        [SerializeField]
        private     Toggle                  m_isLocalPathToggle             = null;

        private     SocialShareComposer     m_activeComposer                = null;

        #endregion

        #region Base class methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            SetActiveComposer(null);
            m_typeDropdown.options  = new List<Dropdown.OptionData>(Array.ConvertAll(Enum.GetNames(typeof(SocialShareComposerType)), (item) => new Dropdown.OptionData(item)));
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "ShareComposer.CreateInstance(shareComposerType)";
        }

        protected override void OnActionSelectInternal(SocialShareComposerDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case SocialShareComposerDemoActionType.IsComposerAvailable:
                    var     selectedComposer1   = GetSelectedComposerType();
                    Log(string.Format("Share composer for type: {0} is available: {1}", selectedComposer1, SocialShareComposer.IsComposerAvailable(selectedComposer1)));
                    break;

                case SocialShareComposerDemoActionType.New:
                    Log("Creating new share composer.");
                    // create instance
                    var     selectedComposer2   = GetSelectedComposerType();
                    var     newComposer         = SocialShareComposer.CreateInstance(selectedComposer2);
                    newComposer.SetCompletionCallback(OnComposerClosed);

                    // save reference
                    SetActiveComposer(newComposer);
                    break;

                case SocialShareComposerDemoActionType.SetText:
                    Log("Setting text.");
                    m_activeComposer.SetText(m_textInputField.text);
                    break;

                case SocialShareComposerDemoActionType.AddScreenshot:
                    Log("Adding screenshot.");
                    m_activeComposer.AddScreenshot();
                    break;

                case SocialShareComposerDemoActionType.AddImage:
                    Log("Adding random image.");
                    var     image   = DemoResources.GetRandomImage();
                    m_activeComposer.AddImage(image);
                    break;

                case SocialShareComposerDemoActionType.AddURL:
                    Log("Adding url.");
                    var     url     = m_isLocalPathToggle.isOn 
                        ? URLString.FileURLWithPath(m_urlInputField.text) 
                        : URLString.URLWithPath(m_urlInputField.text);
                    m_activeComposer.AddURL(url);
                    break;

                case SocialShareComposerDemoActionType.Show:
                    Log("Showing share composer now.");
                    m_activeComposer.Show();
                    break;

                case SocialShareComposerDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kSharingServices);
                    break;

                default:
                    break;
            }
        }

        private void OnComposerClosed(SocialShareComposerResult result, Error error)
        {
            Log("Share composer was closed. Result code: " + result.ResultCode);

            // reset instance
            SetActiveComposer(null);
        }

        #endregion

        #region Private methods

        private SocialShareComposerType GetSelectedComposerType()
        {
            return (SocialShareComposerType)Enum.GetValues(typeof(SocialShareComposerType)).GetValue(m_typeDropdown.value);
        }

        private void SetActiveComposer(SocialShareComposer composer)
        {
            // set property
            m_activeComposer    = composer;

            // update ui
            SetInstanceDependentObjectState(composer != null);
        }

        private void SetInstanceDependentObjectState(bool active)
        {
            foreach (var rect in m_instanceDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }
        }

        #endregion
	}
}