using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class TypeCache
    {
        #region Static fields

        private static Dictionary<Type, string> s_typeMap;
        
        private static bool                     s_isDirty;

        #endregion

        #region Static methods

        public static void Rebuild()
        {
            // Reset properties
            s_isDirty   = true;
            s_typeMap?.Clear();

            EnsureCacheIsUpdated();
        }

        public static Dictionary<MemberInfo, TAttribute> GetMembersWithAttribute<TAttribute>(MemberTypes memberTypes, BindingFlags bindingAttr)
            where TAttribute : Attribute
        {
            return GetMembersWithAttribute<MemberInfo, TAttribute>(memberTypes, bindingAttr);
        }

        public static Dictionary<FieldInfo, TAttribute> GetFieldsWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<FieldInfo, TAttribute>(MemberTypes.Field, bindingAttr);
        }

        public static Dictionary<PropertyInfo, TAttribute> GetPropertiesWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<PropertyInfo, TAttribute>(MemberTypes.Property, bindingAttr);
        }

        public static Dictionary<EventInfo, TAttribute> GetEventsWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<EventInfo, TAttribute>(MemberTypes.Event, bindingAttr);
        }

        public static Dictionary<MethodInfo, TAttribute> GetMethodsWithAttribute<TAttribute>(BindingFlags bindingAttr) where TAttribute : Attribute
        {
            return GetMembersWithAttribute<MethodInfo, TAttribute>(MemberTypes.Method, bindingAttr);
        }

        #endregion

        #region Private methods

        private static void EnsureCacheIsUpdated()
        {
            if (!s_isDirty) return;

            // Initialize object
            s_isDirty       = false;
            if (s_typeMap == null)
            {
                s_typeMap   = new Dictionary<Type, string>(capacity: 1024);
            }

            // Add types to the cache
            foreach (var type in ReflectionUtility.FindAllTypes())
            {
                s_typeMap.Add(type, type.FullName);
            }
        }

        private static Dictionary<TMemberInfo, TAttribute> GetMembersWithAttribute<TMemberInfo, TAttribute>(MemberTypes memberTypes, BindingFlags bindingAttr)
            where TMemberInfo : MemberInfo
            where TAttribute : Attribute
        {
            EnsureCacheIsUpdated();

            // Find all the methods with specified attribute
            var     attributeType   = typeof(TAttribute);
            var     collection      = new Dictionary<TMemberInfo, TAttribute>();
            foreach (var mapItem in s_typeMap)
            {
                var     currentType = mapItem.Key;

                AddMembersWithRequiredAttributes(currentType);

                //When we create a "concrete" class derived from a generic class, it will internally constructs a new class (replacing generic parameters with actual types). We need to query this type for required attributes as well.
                if (IsConstructedClosedGenericType(currentType.BaseType)) 
                {
                    AddMembersWithRequiredAttributes(currentType.BaseType);
                }
            }
            return collection;

            void AddMembersWithRequiredAttributes(Type type)
            {
                var members = type.FindMembers(memberTypes, bindingAttr, null, null);
                foreach (var memberInfo in members)
                {
                    var     attributes  = memberInfo.GetCustomAttributes(attributeType, false);
                    if (attributes.IsNullOrEmpty()) continue;

                    collection.Add(memberInfo as TMemberInfo, attributes[0] as TAttribute);
                }
            }

        }

        private static bool IsConstructedClosedGenericType(Type type)
        {
            return type != null && type.IsConstructedGenericType && !type.ContainsGenericParameters;
        }

        #endregion
    }
}