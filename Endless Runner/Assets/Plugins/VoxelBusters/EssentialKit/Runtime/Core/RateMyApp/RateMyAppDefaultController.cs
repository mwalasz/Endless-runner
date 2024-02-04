using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    public class RateMyAppDefaultController : MonoBehaviour, IRateMyAppController
    {
        #region Fields

        private     RateMyAppDefaultControllerSettings      m_validatorSettings     = null;

        private     ControllerStateInfo                     m_stateInfo             = null;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // initialise component
            var     appSettings     = EssentialKitSettings.Instance.ApplicationSettings;
            m_validatorSettings     = appSettings.RateMyAppSettings.DefaultValidatorSettings;
            m_stateInfo             = LoadStateInfo() ?? new ControllerStateInfo();

            RecordAppLaunch();
        }

        #endregion

        #region Static methods

        private static string SerializeDateTime(DateTime dateTime)
        {
            return dateTime.ToString("O");
        }

        private static DateTime? DeserializeDateTime(string dateTimeStr)
        {
            DateTime    dateTime;
            if (DateTime.TryParse(dateTimeStr, out dateTime))
            {
                return dateTime;
            }
            return null;
        }

        #endregion

        #region Private methods

        private void RecordAppLaunch()
        {
            m_stateInfo.AppLaunchCount++;
            SetDirty();
        }

        private void SetPromptLastShown(DateTime dateTime, bool incrementPromptCount)
        {
            m_stateInfo.PromptLastShown = dateTime;
            if (incrementPromptCount)
            {
                m_stateInfo.PromptCount++;
            }
            SetDirty();
        }

        private bool CheckIfValidatorConditionsAreSatisfied()
        {
            // check whether constraints are satisfied
            var     currentTime         = DateTime.UtcNow;
            var     promptLastShown     = m_stateInfo.PromptLastShown;
            if (promptLastShown == null)
            {
                SetPromptLastShown(currentTime, incrementPromptCount: false);
                return false;
            }

            var     constraints         = (m_stateInfo.PromptCount == 0) ? m_validatorSettings.InitialPromptConstraints : m_validatorSettings.RepeatPromptConstraints;
            if (m_stateInfo.AppLaunchCount > constraints.MinLaunches)
            {
                int     hoursSincePromptLastShown   = (int)(currentTime - promptLastShown.Value).TotalHours;
                if (hoursSincePromptLastShown > constraints.MinHours)
                {
                    SetPromptLastShown(currentTime, incrementPromptCount: true);
                    return true;
                }
            }
            
            return false;
        }

        #endregion

        #region Serialize methods

        private void SetDirty()
        {
            SaveStateInfo(m_stateInfo);
        }

        private const string kPrefKey   = "rma_state";
        private ControllerStateInfo LoadStateInfo()
        {
            string savedValue   = PlayerPrefs.GetString(kPrefKey);
            if (!string.IsNullOrEmpty(savedValue))
            {
                return JsonUtility.FromJson<ControllerStateInfo>(savedValue);
            }
            return null;
        }

        private void SaveStateInfo(ControllerStateInfo stateInfo)
        {
            Assert.IsArgNotNull(stateInfo, "stateInfo");

            string  jsonStr     = JsonUtility.ToJson(stateInfo);
            PlayerPrefs.SetString(kPrefKey, jsonStr);
        }

        #endregion

        #region IRateMyAppValidator implementation

        public bool CanShowRateMyApp()
        {
            // check if user has denied to show
            if (!m_stateInfo.IsActive)
            {
                return false;
            }

            // if we don't want any prompts, -1 is set in settings.
            if (m_validatorSettings.InitialPromptConstraints.MinLaunches == -1 && 
                m_validatorSettings.RepeatPromptConstraints.MinLaunches == -1) 
            {
                return false;
            }
            
            // check if rating is provided already
            var     versionLastRated    = m_stateInfo.VersionLastRated;
            if (!string.IsNullOrEmpty(versionLastRated))
            {
                // check if version matches, then it means app is already reviewed for this version
                string  currentVersion  = Application.version;
                if (string.Compare(currentVersion, versionLastRated, StringComparison.InvariantCulture) <= 0)
                {
                    return false;
                }
            }
            
            return CheckIfValidatorConditionsAreSatisfied();        
        }

        public void DidClickOnRemindLaterButton()
        { }

        public void DidClickOnCancelButton()
        {
            m_stateInfo.IsActive    = false;
            SetDirty();
        }

        public void DidClickOnOkButton()
        {
            m_stateInfo.VersionLastRated    = Application.identifier;
            SetDirty();
        }

        #endregion

        #region Nested types

        [Serializable]
        private class ControllerStateInfo
        {
            #region Fields

            [SerializeField]
            private     string      m_versionLastRated;

            [SerializeField]
            private     int         m_appLaunchCount;
            
            [SerializeField]
            private     string      m_promptLastShown;

            [SerializeField]
            private     int         m_promptCount;

            [SerializeField]
            private     bool        m_isActive;

            #endregion

            #region Properties

            public string VersionLastRated
            {
                get
                {
                    return m_versionLastRated;
                }
                set
                {
                    m_versionLastRated  = value;
                }
            }

            public int AppLaunchCount
            {
                get
                {
                    return m_appLaunchCount;
                }
                set
                {
                    m_appLaunchCount    = value;
                }
            }

            public DateTime? PromptLastShown
            {
                get
                {
                    return string.IsNullOrEmpty(m_promptLastShown) ? null : DeserializeDateTime(m_promptLastShown);
                }
                set
                {
                    m_promptLastShown   = (value == null) ? null : SerializeDateTime(value.Value);
                }
            }

            public int PromptCount
            {
                get
                {
                    return m_promptCount;
                }
                set
                {
                    m_promptCount   = value;
                }
            }

            public bool IsActive
            {
                get
                {
                    return m_isActive;
                }
                set
                {
                    m_isActive  = value;
                }
            }

            #endregion

            #region Constructors

            public ControllerStateInfo()
            {
                // set properties
                m_versionLastRated  = null;
                m_appLaunchCount    = 0;
                m_promptLastShown   = null;
                m_promptCount       = 0;
                m_isActive          = true;
            }

            #endregion
        }

        #endregion
    }
}