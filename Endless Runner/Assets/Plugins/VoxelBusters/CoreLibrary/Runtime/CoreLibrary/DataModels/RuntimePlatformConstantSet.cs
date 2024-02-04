using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [Serializable]
    public class RuntimePlatformConstantSet
    {
        #region Fields

        [SerializeField]
        private     string      m_ios;

        [SerializeField]
        private     string      m_tvos;

        [SerializeField]
        private     string      m_android;

        #endregion

        #region Constructors

        public RuntimePlatformConstantSet(string ios = null, string tvos = null, string android = null)
        {
            // set properties
            m_ios       = ios;
            m_tvos      = tvos;
            m_android   = android;
        }

        #endregion

        #region Public methods

        public string GetConstantForActivePlatform(string defaultValue = null)
        {
            var     platform    = ApplicationServices.GetActivePlatform();
            return GetConstantForPlatform(platform, defaultValue);
        }

        public string GetConstantForActiveOrSimulationPlatform(string defaultValue = null)
        {
            var     platform    = ApplicationServices.GetActiveOrSimulationPlatform();
            return GetConstantForPlatform(platform, defaultValue);
        }

        public string GetConstantForPlatform(RuntimePlatform platform, string defaultValue = null)
        {
            string  targetValue;
            switch (platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    targetValue = m_ios;
                    break;

                case RuntimePlatform.tvOS:
                    targetValue = m_tvos;
                    break;
                    
                case RuntimePlatform.Android:
                    targetValue = m_android;
                    break;

                default:
                    targetValue = defaultValue;
                    break;
            }
            return targetValue;
        }

        #endregion
    }
}