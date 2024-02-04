using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class DefaultValueAttribute : Attribute
    {
        #region Fields

        private     bool?       m_boolValue;

        private     int?        m_int32Value;

        private     float?      m_singleValue;

        private     string      m_stringValue;

        #endregion

        #region Properties

        public bool BoolValue => m_boolValue.GetValueOrDefault();

        public int Int32Value => m_int32Value.GetValueOrDefault();

        public float SingleValue => m_singleValue.GetValueOrDefault();

        public string StringValue => m_stringValue;

        #endregion

        #region Constructors

        public DefaultValueAttribute(bool value)
        {
            // set properties
            m_boolValue     = value;
        }

        public DefaultValueAttribute(int value)
        {
            // set properties
            m_int32Value    = value;
        }

        public DefaultValueAttribute(float value)
        {
            // set properties
            m_singleValue   = value;
        }

        public DefaultValueAttribute(string value)
        {
            // set properties
            m_stringValue   = value;
        }

        #endregion

        #region Public methods

        public T GetValue<T>()
        {
            return (T)GetValue(typeof(T));
        }

        public object GetValue(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return BoolValue;

                case TypeCode.Int32:
                    return Int32Value;

                case TypeCode.Single:
                    return SingleValue;

                case TypeCode.String:
                    return StringValue;

                default:
                    return null;
            }
        }

        #endregion
    }
}