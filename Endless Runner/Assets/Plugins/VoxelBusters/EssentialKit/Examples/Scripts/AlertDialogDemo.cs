using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class AlertDialogDemo : DemoActionPanelBase<AlertDialogDemoAction, AlertDialogDemoActionType>
    {
       #region Fields

        [SerializeField]
        private     InputField          m_titleInputField                   = null;

        [SerializeField]    
        private     InputField          m_messageInputField                 = null;

        [SerializeField]
        private     InputField          m_buttonLabelInputField             = null;

        [SerializeField]
        private     InputField          m_cancelButtonLabelInputField       = null;

        [SerializeField]
        private     RectTransform       m_configureSection                  = null;

        [SerializeField]
        private     RectTransform       m_presentationSection               = null;    

        private     AlertDialog         m_activeDialog;

        #endregion

        #region Base methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            SetConfigureState(false);
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "AlertDialog.CreateInstance()";
        }

        protected override void OnActionSelectInternal(AlertDialogDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
               case AlertDialogDemoActionType.New:
                    Log("Creating new alert dialog.");
                    m_activeDialog          = AlertDialog.CreateInstance();
                    SetConfigureState(true);
                    break;

                case AlertDialogDemoActionType.SetTitle:
                    Log("Setting alert title.");
                    m_activeDialog.Title    = m_titleInputField.text;
                    break;

                case AlertDialogDemoActionType.GetTitle:
                    Log("Alert title: " + m_activeDialog.Title);
                    break;

                case AlertDialogDemoActionType.SetMessage:
                    Log("Setting alert message.");
                    m_activeDialog.Message  = m_messageInputField.text;
                    break;

                case AlertDialogDemoActionType.GetMessage:
                    Log("Alert message: " + m_activeDialog.Message);
                    break;

                case AlertDialogDemoActionType.AddButton:
                    string  buttonLabel0    = m_buttonLabelInputField.text;
                    Log("Adding button with label: " + buttonLabel0);
                    m_activeDialog.AddButton(buttonLabel0, () =>
                    {
                        Log(string.Format("Button with label: {0} is selected", buttonLabel0));

                        // reset
                        m_activeDialog      = null;
                        SetConfigureState(false);
                    });
                    break;

                case AlertDialogDemoActionType.AddCancelButton:
                    string  buttonLabel1    = m_cancelButtonLabelInputField.text;
                    Log("Adding button with label: " + buttonLabel1);
                    m_activeDialog.AddCancelButton(buttonLabel1, () =>
                    {
                        Log(string.Format("Button with label: {0} is selected", buttonLabel1));

                        // reset
                        m_activeDialog      = null;
                        SetConfigureState(false);
                    });
                    break;

                case AlertDialogDemoActionType.Show:
                    Log("Showing alert dialog.");
                    m_activeDialog.Show();
                    break;

                case AlertDialogDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kNativeUI);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Private methods

        public void SetConfigureState(bool value)
        {
            m_configureSection.gameObject.SetActive(value);
            m_presentationSection.gameObject.SetActive(value);
        }

        #endregion
    }
}