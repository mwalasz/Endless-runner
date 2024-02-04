using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [SerializeField]
    public abstract class SettingsPropertyGroup
    {
        #region Fields

        [SerializeField]
        private     bool        m_isEnabled     = true;

        #endregion

        #region Properties

        public bool IsEnabled
        {
            get
            {
#if UNITY_EDITOR
                // update status value, considering editor support for selective inclusion of feature set
                /*if (!m_isEnabled && !CanToggleFeatureUsageState())
                {
                    Debug.LogWarning(string.Format("Resetting feature: {0} state to active because current build configuration provides limited support for stripping code. Check EssentialKitSettings file for fix.", Name));
                    m_isEnabled     = true;
                }*/
#endif
                return m_isEnabled;
            }
            set
            {
#if UNITY_EDITOR
                if (!value && !CanToggleFeatureUsageState())
                {
                    Debug.LogWarning(string.Format("Ignoring feature: {0} state value change request because current build configuration provides limited support for stripping code. Check EssentialKitSettings file for fix.", Name));
                    return;
                }
                m_isEnabled = value;
#endif
            }
        }

        public string Name { get; private set; }

        #endregion

        #region Constructors

        protected SettingsPropertyGroup(string name, bool isEnabled = true)
        {
            // set properties
            m_isEnabled     = isEnabled;
            Name            = name;
        }

        #endregion

        #region Static methods

#if UNITY_EDITOR
        public static bool CanToggleFeatureUsageState()
        {
            var     target          = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            var     targetGroup     = UnityEditor.BuildPipeline.GetBuildTargetGroup(target);
            var     strippingLevel  = UnityEditor.PlayerSettings.GetManagedStrippingLevel(targetGroup);
#if !UNITY_2019_3_OR_NEWER
            return (UnityEditor.PlayerSettings.scriptingRuntimeVersion == UnityEditor.ScriptingRuntimeVersion.Latest) && ((strippingLevel == UnityEditor.ManagedStrippingLevel.Medium) || (strippingLevel == UnityEditor.ManagedStrippingLevel.High));
#else
            return (strippingLevel == UnityEditor.ManagedStrippingLevel.Medium) || (strippingLevel == UnityEditor.ManagedStrippingLevel.High);
#endif
        }

        public static void UpdateBuildConfigurationToSupportToggleFeatureUsageState()
        {
            var     target          = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            var     targetGroup     = UnityEditor.BuildPipeline.GetBuildTargetGroup(target);
            var     strippingLevel  = UnityEditor.PlayerSettings.GetManagedStrippingLevel(targetGroup);
#if !UNITY_2019_3_OR_NEWER
            if (UnityEditor.PlayerSettings.scriptingRuntimeVersion != UnityEditor.ScriptingRuntimeVersion.Latest)
            {
                Debug.Log("[VoxelBusters] Scripting runtime version changed to \'Latest\'. Kindly relaunch your editor.");
                UnityEditor.PlayerSettings.scriptingRuntimeVersion = UnityEditor.ScriptingRuntimeVersion.Latest;
            }
#endif
            if ((strippingLevel == UnityEditor.ManagedStrippingLevel.Disabled) || (strippingLevel == UnityEditor.ManagedStrippingLevel.Low))
            {
                Debug.Log("[VoxelBusters] Managed stripping level changed to \'Medium\'.");
                UnityEditor.PlayerSettings.SetManagedStrippingLevel(targetGroup, UnityEditor.ManagedStrippingLevel.Medium);
            }
        }
#endif

        #endregion
    }
}