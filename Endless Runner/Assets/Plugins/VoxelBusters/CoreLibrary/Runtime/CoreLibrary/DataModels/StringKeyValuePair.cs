using System;

namespace VoxelBusters.CoreLibrary
{
    [Serializable]
    public class StringKeyValuePair : SerializableKeyValuePair<string, string>
    {
        #region Constructors

        public StringKeyValuePair(string key = null, string value = null)
            : base(key, value)
        { }

        #endregion
    }
}