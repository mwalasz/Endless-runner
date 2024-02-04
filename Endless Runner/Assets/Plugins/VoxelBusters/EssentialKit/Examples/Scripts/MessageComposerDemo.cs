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
	public class MessageComposerDemo : DemoActionPanelBase<MessageComposerDemoAction, MessageComposerDemoActionType>
	{
        #region Fields

        [SerializeField]
        private     RectTransform[]     m_instanceDependentObjects      = null;

        [SerializeField]
        private     InputField          m_recipientsInputField          = null;

        [SerializeField]
        private     InputField          m_subjectInputField             = null;

        [SerializeField]
        private     InputField          m_bodyInputField                = null;

        private     MessageComposer     m_activeComposer                = null;

        #endregion

        #region Base methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            SetActiveComposer(null);
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "MessageComposer.CreateInstance()";
        }

        protected override void OnActionSelectInternal(MessageComposerDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case MessageComposerDemoActionType.CanSendText:
                    Log("Can send text: " + MessageComposer.CanSendText());
                    break;

                case MessageComposerDemoActionType.New:
                    Log("Creating new message composer.");
                    // create new instance
                    var     newComposer     = MessageComposer.CreateInstance();
                    newComposer.SetCompletionCallback(OnComposerClosed);

                    // save reference
                    SetActiveComposer(newComposer);
                    break;

                case MessageComposerDemoActionType.SetRecipients:
                    Log("Setting recipients.");
                    m_activeComposer.SetRecipients(DemoHelper.GetMultivaluedString(m_recipientsInputField));
                    break;

                case MessageComposerDemoActionType.SetSubject:
                    Log("Setting subject.");
                    m_activeComposer.SetSubject(m_subjectInputField.text);
                    break;

                case MessageComposerDemoActionType.SetBody:
                    Log("Setting body.");
                    m_activeComposer.SetBody(m_bodyInputField.text);
                    break;

                case MessageComposerDemoActionType.Show:
                    Log("Showing message composer now.");
                    m_activeComposer.Show();
                    break;

                case MessageComposerDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kSharingServices);
                    break;

                default:
                    break;
            }
        }

        private void OnComposerClosed(MessageComposerResult result, Error error)
        {
            Log("Message composer was closed. Result code: " + result.ResultCode);

            // reset instance
            SetActiveComposer(null);
        }

        #endregion

        #region Private methods

        private void SetActiveComposer(MessageComposer composer)
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
