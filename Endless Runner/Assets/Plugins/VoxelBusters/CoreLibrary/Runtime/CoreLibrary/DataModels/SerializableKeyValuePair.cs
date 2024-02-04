using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue> : IEquatable<SerializableKeyValuePair<TKey, TValue>>
    {
        #region Fields

        [SerializeField]
        private     TKey        m_key;

        [SerializeField]
        private     TValue      m_value;

        #endregion

        #region Properties

        public TKey Key
        {
            get => m_key;
            set => m_key    = value;
        }

        public TValue Value
        {
            get => m_value;
            set => m_value  = value;
        }

        #endregion

        #region Constructors

        protected SerializableKeyValuePair(TKey key = default(TKey), TValue value = default(TValue))
        {
            // set properties
            m_key       = key;
            m_value     = value;
        }

        #endregion

        #region IEquatable implementation

        bool IEquatable<SerializableKeyValuePair<TKey, TValue>>.Equals(SerializableKeyValuePair<TKey, TValue> other)
        {
            if (other == null) return false;

            return EqualityComparer<TKey>.Default.Equals(Key, other.Key) &&
                EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        #endregion
    }
}