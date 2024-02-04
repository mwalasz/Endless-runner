using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ObjectHelper
    {
        #region Static methods

        public static T CreateInstanceIfNull<T>(ref T reference, System.Func<T> createFunc) where T : class
        {
            if (reference == null)
            {
                reference   = createFunc();
            }
            return reference;
        }

        #endregion
    }
}