using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

// Credits: https://github.com/joshcamas/unity-domain-reload-helper
namespace VoxelBusters.CoreLibrary.Editor
{
    public static class DomainReloadManager
    {
        #region Static fields

        private const   BindingFlags    kDefaultBindingAttr     = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        #endregion

        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnRuntimeLoad()
        {
            TypeCache.Rebuild();

            SendReloadMessageToMembers();
            SendReloadMessageToMethods(); 
        }

        private static void SendReloadMessageToMembers()
        {
            var     members = TypeCache.GetMembersWithAttribute<ClearOnReloadAttribute>(
                memberTypes: MemberTypes.Field | MemberTypes.Property | MemberTypes.Event,
                bindingAttr: kDefaultBindingAttr);
            foreach (var item in members)
            {
                var     member      = item.Key;
                var     attribute   = item.Value;
                if (member is FieldInfo)
                {
                    ClearField(member as FieldInfo, attribute);
                }
                else if (member is PropertyInfo)
                {
                    ClearProperty(member as PropertyInfo, attribute);
                }
                else if (member is EventInfo)
                {
                    ClearEvent(member as EventInfo, attribute);
                }
            }
        }

        private static void SendReloadMessageToMethods()
        {
            foreach (var item in TypeCache.GetMethodsWithAttribute<ExecuteOnReloadAttribute>(kDefaultBindingAttr))
            {
                var     methodInfo  = item.Key;
                if (methodInfo.IsGenericMethod) continue;

                DebugLogger.Log(CoreLibraryDomain.Default, $"Invoking reload method: {methodInfo.Name}.");
                methodInfo.Invoke(null, new object[] { });
            }
        }

        private static void ClearField(FieldInfo fieldInfo, ClearOnReloadAttribute clearAttribute)
        {
            var     fieldType   = fieldInfo.FieldType;
            if (fieldType.IsGenericParameter || fieldInfo.DeclaringType.ContainsGenericParameters)
            {
                return;
            }

            // Access attribute parameters
            object  resetValue  = FindMemberResetValue(fieldType, clearAttribute);

            try
            {
                DebugLogger.Log(CoreLibraryDomain.Default, $"Clearing field {fieldInfo.Name} to value {resetValue}.");
                fieldInfo.SetValue(null, resetValue);
            }
            catch(Exception e)
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, $"Unable to clear field {fieldInfo.Name}." + e);
            }
        }

        private static void ClearProperty(PropertyInfo propertyInfo, ClearOnReloadAttribute clearAttribute)
        {
            if (!propertyInfo.CanWrite) return;

            // Access attribute parameters
            var     propertyType    = propertyInfo.PropertyType;
            object  resetValue      = FindMemberResetValue(propertyType, clearAttribute);

            try
            {
                DebugLogger.Log(CoreLibraryDomain.Default, $"Clearing property {propertyInfo.Name} to value {resetValue}.");
                propertyInfo.SetValue(null, resetValue);
            }
            catch
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, $"Unable to clear property {propertyInfo.Name}.");
            }
        }

        private static void ClearEvent(EventInfo eventInfo, ClearOnReloadAttribute clearAttribute)
        {
            var     fieldInfo   = eventInfo.DeclaringType.GetField(eventInfo.Name, kDefaultBindingAttr);
            if ((fieldInfo == null) || fieldInfo.FieldType.IsGenericParameter) return;

            DebugLogger.Log(CoreLibraryDomain.Default, $"Clearing event {eventInfo.Name}.");
            fieldInfo.SetValue(null, null);
        }

        private static object FindMemberResetValue(Type target, ClearOnReloadAttribute clearAttribute)
        {
            object  finalValue  = null;
            if (ClearOnReloadOption.Custom == clearAttribute.Option)
            {
                finalValue      = Convert.ChangeType(clearAttribute.CustomValue, target);
                if (finalValue == null)
                {
                    Debug.LogWarning("Unable to assign value of type {valueToAssign.GetType()} to field {field.Name} of type {fieldType}.");
                }
            }
            else if (target.IsValueType || (ClearOnReloadOption.Default == clearAttribute.Option))
            {
                finalValue      = Activator.CreateInstance(target);
            }
            return finalValue;
        }

        #endregion
    }
}