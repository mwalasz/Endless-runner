using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class RateMyAppConfirmationDialogSettings 
    {
        #region Fields

        [SerializeField, DefaultValue(true)]
        [Tooltip("If enabled, confirmation dialog is shown prior to prompting rating window.")]
        private     bool                m_canShow;

        [SerializeField, DefaultValue("Rate My App")]
        [Tooltip("Title.")]
        private     string              m_promptTitle;

        [SerializeField, TextArea, DefaultValue("If you enjoy using Native Plugins would you mind taking a moment to rate it? It wont take more than a minute. Thanks for your support.")]
        [Tooltip("Description.")]
        private     string              m_promptDescription;

        [SerializeField, DefaultValue("Ok")]
        [Tooltip("Positive action button label.")]
        private     string              m_okButtonLabel;

        [SerializeField, DefaultValue("Cancel")]
        [Tooltip("Negative action button label.")]
        private     string              m_cancelButtonLabel;

        [SerializeField, DefaultValue("Remind Me Later")]
        [Tooltip("Neutral action button label.")]
        private     string              m_remindLaterButtonLabel;

        [SerializeField, DefaultValue(true)]
        [Tooltip("Determines whether neutral action button is required.")]
        private     bool                m_canShowRemindMeLaterButton;

        #endregion

        #region Properties

        public bool CanShow => m_canShow;

        public string PromptTitle => PropertyHelper.GetValueOrDefault(
            instance: this,
            fieldAccess: (field) => field.m_promptTitle,
            value: m_promptTitle);

        public string PromptDescription => PropertyHelper.GetValueOrDefault(
            instance: this,
            fieldAccess: (field) => field.m_promptDescription,
            value: m_promptDescription);

        public string OkButtonLabel => PropertyHelper.GetValueOrDefault(
            instance: this,
            fieldAccess: (field) => field.m_okButtonLabel,
            value: m_okButtonLabel);

        public string CancelButtonLabel => PropertyHelper.GetValueOrDefault(
            instance: this,
            fieldAccess: (field) => field.m_cancelButtonLabel,
            value: m_cancelButtonLabel);

        public string RemindLaterButtonLabel => PropertyHelper.GetValueOrDefault(
            instance: this,
            fieldAccess: (field) => field.m_remindLaterButtonLabel,
            value: m_remindLaterButtonLabel);

        public bool CanShowRemindMeLaterButton => m_canShowRemindMeLaterButton;

        #endregion

        #region Constructors

        public RateMyAppConfirmationDialogSettings(bool canShow = true, string title = null,
            string description = null, string okButtonLabel = null,
            string cancelButtonLabel = null, string remindLaterButtonLabel = null,
            bool canShowRemindMeLaterButton = true)
        {
            // set properties
            m_canShow                       = canShow;
            m_promptTitle                   = PropertyHelper.GetValueOrDefault(
                instance: this,
                fieldAccess: (field) => field.m_promptTitle,
                value: title);
            m_promptDescription             = PropertyHelper.GetValueOrDefault(
                instance: this,
                fieldAccess: (field) => field.m_promptDescription,
                value: description);
            m_okButtonLabel                 = PropertyHelper.GetValueOrDefault(
                instance: this,
                fieldAccess: (field) => field.m_okButtonLabel,
                value: okButtonLabel);
            m_cancelButtonLabel             = PropertyHelper.GetValueOrDefault(
                instance: this,
                fieldAccess: (field) => field.m_cancelButtonLabel,
                value: cancelButtonLabel);
            m_remindLaterButtonLabel        = PropertyHelper.GetValueOrDefault(
                instance: this,
                fieldAccess: (field) => field.m_remindLaterButtonLabel,
                value: remindLaterButtonLabel);
            m_canShowRemindMeLaterButton    = canShowRemindMeLaterButton;
        }

        #endregion
    }
}