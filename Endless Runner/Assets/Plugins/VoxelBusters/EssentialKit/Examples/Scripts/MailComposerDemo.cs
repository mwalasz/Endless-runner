using System.Text;
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
    public class MailComposerDemo : DemoActionPanelBase<MailComposerDemoAction, MailComposerDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     RectTransform[]     m_instanceDependentObjects      = null;

        [SerializeField]
        private     InputField          m_toInputField                  = null;

        [SerializeField]
        private     InputField          m_ccInputField                  = null;

        [SerializeField]
        private     InputField          m_bccInputField                 = null;

        [SerializeField]
        private     InputField          m_subjectInputField             = null;

        [SerializeField]
        private     InputField          m_bodyInputField                = null;

        [SerializeField]
        private     Toggle              m_isHtmlToggle                  = null;

        private     MailComposer        m_activeComposer                = null;

        private     int                 m_attachmentCounter             = 0;

        #endregion

        #region Base class methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            SetActiveComposer(null);
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "MailComposer.CreateInstance()";
        }

        protected override void OnActionSelectInternal(MailComposerDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case MailComposerDemoActionType.CanSendMail:
                    Log("Can send mail: " + MailComposer.CanSendMail());
                    break;

                case MailComposerDemoActionType.New:
                    Log("Creating new mail composer.");
                    // create instance
                    var     newComposer     = MailComposer.CreateInstance();                    
                    newComposer.SetCompletionCallback(OnComposerClosed);

                    // save reference
                    SetActiveComposer(newComposer);
                    break;

                case MailComposerDemoActionType.SetToRecipients:
                    Log("Setting To recipients.");
                    m_activeComposer.SetToRecipients(DemoHelper.GetMultivaluedString(m_toInputField));
                    break;

                case MailComposerDemoActionType.SetCcRecipients:
                    Log("Setting Cc recipients.");
                    m_activeComposer.SetCcRecipients(DemoHelper.GetMultivaluedString(m_ccInputField));
                    break;

                case MailComposerDemoActionType.SetBccRecipients:
                    Log("Setting Bcc recipients.");
                    m_activeComposer.SetBccRecipients(DemoHelper.GetMultivaluedString(m_bccInputField));
                    break;

                case MailComposerDemoActionType.SetSubject:
                    Log("Setting subject.");
                    m_activeComposer.SetSubject(m_subjectInputField.text);
                    break;

                case MailComposerDemoActionType.SetBody:
                    Log("Setting body.");
                    m_activeComposer.SetBody(m_bodyInputField.text, m_isHtmlToggle.isOn);
                    break;

                case MailComposerDemoActionType.AddScreenshot:
                    Log("Adding screenshot.");
                    m_activeComposer.AddScreenshot(GetFileName("png"));
                    m_attachmentCounter++;
                    break;

                case MailComposerDemoActionType.AddImage:
                    Log("Adding random image.");
                    var     image   = DemoResources.GetRandomImage();
                    m_activeComposer.AddImage(image, GetFileName("png"));
                    m_attachmentCounter++;
                    break;

                case MailComposerDemoActionType.Show:
                    Log("Showing mail composer now.");
                    m_activeComposer.Show();
                    break;

                case MailComposerDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kSharingServices);
                    break;

                default:
                    break;
            }
        }

        private void OnComposerClosed(MailComposerResult result, Error error)
        {
            Log("Mail composer was closed. Result code: " + result.ResultCode);

            // reset instance
            SetActiveComposer(null);
        }

        #endregion

        #region Private methods

        private string GetFileName(string ext)
        {
            return string.Format("File{0}.{1}", m_attachmentCounter, ext);
        }

        private void SetActiveComposer(MailComposer composer)
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