using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [Serializable]
    public class NativeFeatureUsagePermissionDefinition
    {
        #region Fields

        [SerializeField]
        private     string                      m_description;
        
        [SerializeField]
        private     RuntimePlatformConstantSet  m_descriptionOverrides;

        #endregion

        #region Constructors

        public NativeFeatureUsagePermissionDefinition(string description = null, RuntimePlatformConstantSet descriptionOverrides = null)
        {
            // set properties
            m_description               = description;     
            m_descriptionOverrides      = descriptionOverrides ?? new RuntimePlatformConstantSet();
        }

        #endregion

        #region Public methods

        public string GetDescriptionForActivePlatform()
        {
            return GetDescription(ApplicationServices.GetActiveOrSimulationPlatform());
        }

        public string GetDescription(RuntimePlatform platform)
        {
            // check whether overrides are available
            string  targetValue     = m_descriptionOverrides.GetConstantForPlatform(platform, m_description);
            if (targetValue == null)
            {
                DebugLogger.LogError(CoreLibraryDomain.NativePlugins, "Permission is not defined.");
                return null;
            }
            else
            {
                return FormatDescription(targetValue, platform);
            }
        }

        #endregion

        #region Private methods

        private string FormatDescription(string description, RuntimePlatform targetPlatform)
        {
            switch (targetPlatform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.tvOS:
                    return description.Replace("$productName", "$(PRODUCT_NAME)");

                case RuntimePlatform.Android:
                    return description.Replace("$productName", "%app_name%");

                default:
                    return description;
            }
        }

        #endregion
    }
}