#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.iOS.Xcode;
using UnityEngine;

using CapabilityType = UnityEditor.iOS.Xcode.PBXCapabilityType;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{ 
    public static class ProjectCapabilityManagerExtensions
    {
        private const BindingFlags NonPublicInstanceBinding = BindingFlags.NonPublic | BindingFlags.Instance;

        public static void ForceAddInAppPurchase(this ProjectCapabilityManager manager)
        {
            AddCapability(manager, "com.apple.InAppPurchase", true, "StoreKit.framework", false);
        }

        public static void ForceAddGameCenter(this ProjectCapabilityManager manager)
        {
            AddCapability(manager, "com.apple.GameCenter.iOS", true, "GameKit.framework", false);
        }

        private static void AddCapability(this ProjectCapabilityManager manager, string id, bool requiresEntitlements, string framework = "", bool optionalFramework = false)
        {
            var     managerType         = typeof(ProjectCapabilityManager);
            var     projectField        = managerType.GetField("project", NonPublicInstanceBinding);
            var     targetGuidField     = managerType.GetField("m_TargetGuid", NonPublicInstanceBinding);

            if (projectField == null || targetGuidField == null) return;

            var     project             = projectField.GetValue(manager) as PBXProject;
            if (project != null)
            {
                var     mainTargetGuid      = targetGuidField.GetValue(manager) as string;
                var     type                = typeof(CapabilityType);
                var     types               = new [] { typeof(string), typeof(bool), typeof(string), typeof (bool) };
                var     parameters          = new object[] { id, requiresEntitlements, framework, optionalFramework };
                var     pbxCapabilityType   = (CapabilityType)type.GetConstructor(NonPublicInstanceBinding, null, types, null).Invoke(parameters);
                project.AddCapability(mainTargetGuid, pbxCapabilityType);
            }
            
        }
    }
}
#endif