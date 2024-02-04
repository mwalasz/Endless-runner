using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [Serializable, Obsolete("This class is deprecated. Instead use RuntimePlatformConstantSet.", true)]
    public class NativePlatformConstantSet
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

        public NativePlatformConstantSet(string ios = null, string tvos = null, string android = null)
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
            var     platform    = PlatformMappingServices.GetActivePlatform();
            return GetConstantForPlatform(platform, defaultValue);
        }

        public string GetConstantForPlatform(NativePlatform platform, string defaultValue = null)
        {
            string  targetValue = null;
            switch (platform)
            {
                case NativePlatform.iOS:
                    targetValue = m_ios;
                    break;

                case NativePlatform.tvOS:
                    targetValue = m_tvos;
                    break;
                    
                case NativePlatform.Android:
                    targetValue = m_android;
                    break;

                default:
                    throw VBException.SwitchCaseNotImplemented(platform);
            }

            return string.IsNullOrEmpty(targetValue) ? defaultValue : targetValue;
        }

        #endregion
    }
}