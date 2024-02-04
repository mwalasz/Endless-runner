using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
	public static class SystemAssemblyName
	{
        public  const   string  kCSharpFirstPass    = "Assembly-CSharp-firstpass";
        public  const   string  kCSharp             = "Assembly-CSharp";
	}

    public static class ReflectionUtility
    {
        #region Type methods

		[System.Obsolete("Use method GetTypeFromAssemblyCSharp instead.", false)]
		public static Type GetTypeFromCSharpAssembly(string typeName)
		{
			return GetType(SystemAssemblyName.kCSharp, typeName);
		}

		[System.Obsolete("Use method GetTypeFromAssemblyCSharp instead.", false)]
        public static Type GetTypeFromCSharpFirstPassAssembly(string typeName)
        {
			return GetType(SystemAssemblyName.kCSharpFirstPass, typeName);
        }

        public static Type GetTypeFromAssemblyCSharp(string typeName, bool includeFirstPass = false)
        {
			Type	targetType		= null;
            if (includeFirstPass)
			{
				targetType			= GetType(SystemAssemblyName.kCSharpFirstPass, typeName);
			};
			if (targetType == null)
			{
				targetType			= GetType(SystemAssemblyName.kCSharp, typeName);
			}
			return targetType;
        }

        public static Type GetType(string assemblyName, string typeName)
        {
            var		targetAssembly	= FindAssemblyWithName(assemblyName);
            if (targetAssembly != null)
            {
                return targetAssembly.GetType(typeName, false);
            }

            return null;
        }

        public static Assembly FindAssemblyWithName(string assemblyName)
        {
            return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), (item) =>
            {
                return string.Equals(item.GetName().Name, assemblyName);
            });
        }

		public static Type[] FindAllTypes(Predicate<Type> predicate = null)
        {
			var		typeList	= new List<Type>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if ((predicate == null) || predicate(type))
					{
						typeList.Add(type);
					}
				}
			}
            return typeList.ToArray();
        }

        public static bool TryGetCustomAttriute(this MethodInfo methodInfo,
												Type attributeType,
												out Attribute attribute,
												bool inherit = false)
		{
			// Set default value
			attribute	= null;

			// Find custom attributes
			var     attributes	= methodInfo.GetCustomAttributes(attributeType, inherit);
            if (attributes.Length > 0)
			{
				attribute		= attributes[0] as Attribute;
				return true;
			}
			return false;
		}

		public static bool TryGetCustomAttriute<TAttribute>(this MethodInfo methodInfo,
															out TAttribute attribute,
															bool inherit = false) where TAttribute : Attribute
		{
			bool	value	= TryGetCustomAttriute(methodInfo: methodInfo,
												   attributeType: typeof(TAttribute),
												   attribute: out Attribute genericAttr,
												   inherit: inherit);
			attribute		= genericAttr as TAttribute;
			return value;
		}

		#endregion

		#region Create instance methods

		public static object CreateInstance(Type type, bool nonPublic = true)
		{
			return Activator.CreateInstance(type, nonPublic);
		}

		public static object CreateInstance(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, args);
		}

        #endregion

        #region Attribute methods

		public static Dictionary<Type, Attribute[]> FindTypesWithAttribute(Type attributeType, bool inherit = false)
		{
			var		collection	= new Dictionary<Type, Attribute[]>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
				foreach (var type in assembly.GetTypes())
                {
					if (type.IsClass == false) continue;

					var		customAttributes	= type.GetCustomAttributes(attributeType, inherit);
					if (customAttributes.Length == 0) continue;

					collection.Add(type, Array.ConvertAll(customAttributes, (item) => item as Attribute));
                }
            }
			return collection;
		}

		/*
		public static Type[] FindTypesWithAttributes(Type attributeType, System.Func<Type, bool> typeFilter = null,
			System.Func<MethodInfo> methodFilter = null)
		{
			var		targetTypes	= new List<Type>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
				foreach (var type in assembly.GetTypes())
                {
                    if ((typeFilter != null) && !typeFilter(type)) continue;

					foreach (var method in type.GetRuntimeMethods())
                    {

                    }

                }
            }

			return targetTypes.ToArray();
		}
		*/

        #endregion

        #region Invoke methods

		public static object InvokeStaticMethod(this Type type, string method, params object[] parameters)
        {
            return type.GetMethod(method, BindingFlags.Public | BindingFlags.Static).Invoke(null, parameters);
        }

        public static T InvokeStaticMethod<T>(this Type type, string method, params object[] parameters)
        {
            return (T)InvokeStaticMethod(type, method, parameters);
        }

        #endregion

        #region Modifier methods

		public static void SetPropertyValue(this object obj, string name, object value)
		{
			var		type		= obj.GetType();
			var		bindingAttr	= BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var		property	= type.GetProperty(name, bindingAttr);
			if (property != null)
			{
				if (property.DeclaringType != type)
				{
					property.DeclaringType.GetProperty(name, bindingAttr).SetValue(obj, value);
				}
				else
				{
					property.SetValue(obj, value);
				}
			}
		}

        #endregion

        #region Constraints methods

        // Credits: Thomas Hourdel
        public static string GetFieldName<TInstance, TProperty>(Expression<Func<TInstance, TProperty>> fieldAccess)
		{
			var		memberExpression	= fieldAccess.Body as MemberExpression;
			if (memberExpression != null)
			{
				return memberExpression.Member.Name;
			}
			throw new InvalidOperationException("Member expression expected");
		}

		public static FieldInfo GetField(Type type, string fieldName)
		{
			return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public static TAttribute GetAttribute<TAttribute>(FieldInfo field) where TAttribute : Attribute
		{
			return (TAttribute)Attribute.GetCustomAttribute(field, typeof(TAttribute));
		}

		// MinAttribute
		public static void ConstrainMin<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, float value)
		{
			var		fieldName	= GetFieldName(fieldAccess);
			var		fieldInfo	= GetField(typeof(TInstance), fieldName);
			fieldInfo.SetValue(instance, Mathf.Max(value, GetAttribute<MinAttribute>(fieldInfo).min));
		}

		public static void ConstrainMin<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, int value)
		{
			var		fieldName	= GetFieldName(fieldAccess);
			var		fieldInfo	= GetField(typeof(TInstance), fieldName);
			fieldInfo.SetValue(instance, (int)Mathf.Max(value, GetAttribute<MinAttribute>(fieldInfo).min));
		}

		// RangeAttribute
		public static void ConstrainRange<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, float value)
		{
			var		fieldName	= GetFieldName(fieldAccess);
			var		fieldInfo	= GetField(typeof(TInstance), fieldName);
			var		attribute	= GetAttribute<RangeAttribute>(fieldInfo);
			fieldInfo.SetValue(instance, Mathf.Clamp(value, attribute.min, attribute.max));
		}

		public static void ConstrainRange<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, int value)
		{
			var		fieldName	= GetFieldName(fieldAccess);
			var		fieldInfo	= GetField(typeof(TInstance), fieldName);
			var		attribute	= GetAttribute<RangeAttribute>(fieldInfo);
			fieldInfo.SetValue(instance, (int)Mathf.Clamp(value, attribute.min, attribute.max));
		}

		public static void ConstrainDefault<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, Func<bool> condition = null)
		{
			if ((condition != null) && !condition()) return;

			var		fieldName	= GetFieldName(fieldAccess);
			var		fieldInfo	= GetField(typeof(TInstance), fieldName);
			var		attribute	= GetAttribute<DefaultValueAttribute>(fieldInfo);
			if (attribute != null)
			{
				fieldInfo.SetValue(instance, attribute.GetValue(fieldInfo.FieldType));
			}
		}

		public static void ConstrainDefault<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, string value)
		{
			if (!string.IsNullOrEmpty(value)) return;

			ConstrainDefault(instance, fieldAccess);
		}

        #endregion
    }
}