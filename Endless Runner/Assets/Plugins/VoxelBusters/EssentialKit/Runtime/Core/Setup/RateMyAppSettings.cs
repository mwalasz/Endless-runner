using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class RateMyAppSettings
    {
        #region Fields

        [SerializeField]
        [Tooltip("If enabled, this feature is marked as required.")]
        private     bool                                m_isEnabled;

        [SerializeField]
        [Tooltip("Confirmation dialog settings.")]
        private     RateMyAppConfirmationDialogSettings m_confirmationDialogSettings;

        [SerializeField]
        [Tooltip("Default controller attributes.")]
        private     RateMyAppDefaultControllerSettings  m_defaultControllerSettings;

        #endregion

        #region Properties

        public bool IsEnabled => m_isEnabled;

        public RateMyAppConfirmationDialogSettings ConfirmationDialogSettings => m_confirmationDialogSettings;

        public RateMyAppDefaultControllerSettings DefaultValidatorSettings => m_defaultControllerSettings;

        #endregion

        #region Constructors

        public RateMyAppSettings(bool isEnabled = true, RateMyAppConfirmationDialogSettings dialogSettings = null,
            RateMyAppDefaultControllerSettings defaultValidatorSettings = null)
        {
            // set properties
            m_isEnabled                     = isEnabled;
            m_confirmationDialogSettings    = dialogSettings ?? new RateMyAppConfirmationDialogSettings();
            m_defaultControllerSettings     = defaultValidatorSettings ?? new RateMyAppDefaultControllerSettings();
        }

        #endregion
    }
}