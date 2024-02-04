using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class PropertyHelper
    {
        #region Static methods

        public static string GetValueOrDefault(string value, string defaultValue = default)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;

            return value;
        }

        public static string GetValueOrDefault<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, string value)
        {
            if (!string.IsNullOrEmpty(value)) return value;

            // Find default value using reflection
            var		fieldName	= ReflectionUtility.GetFieldName(fieldAccess);
			var		fieldInfo	= ReflectionUtility.GetField(typeof(TInstance), fieldName);
			var		attribute	= ReflectionUtility.GetAttribute<DefaultValueAttribute>(fieldInfo);
            return attribute.StringValue;
        }

        public static TValue GetValueOrDefault<TInstance, TProperty, TValue>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, Nullable<TValue> value) where TValue : struct
        {
            if (value != null) return value.GetValueOrDefault();

            // Find default value using reflection
            var		fieldName	= ReflectionUtility.GetFieldName(fieldAccess);
			var		fieldInfo	= ReflectionUtility.GetField(typeof(TInstance), fieldName);
			var		attribute	= ReflectionUtility.GetAttribute<DefaultValueAttribute>(fieldInfo);
            return attribute.GetValue<TValue>();
        }

        public static int GetConstrainedValue<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, int value)
        {
            var		fieldName	= ReflectionUtility.GetFieldName(fieldAccess);
			var		fieldInfo	= ReflectionUtility.GetField(typeof(TInstance), fieldName);
			var		attribute	= ReflectionUtility.GetAttribute<RangeAttribute>(fieldInfo);
            return Mathf.Clamp(value, (int)attribute.min, (int)attribute.max);
        }

        public static float GetConstrainedValue<TInstance, TProperty>(TInstance instance, Expression<Func<TInstance, TProperty>> fieldAccess, float value)
        {
            var		fieldName	= ReflectionUtility.GetFieldName(fieldAccess);
			var		fieldInfo	= ReflectionUtility.GetField(typeof(TInstance), fieldName);
			var		attribute	= ReflectionUtility.GetAttribute<RangeAttribute>(fieldInfo);
            return Mathf.Clamp(value, attribute.min, attribute.max);
        }

        #endregion
    }
}