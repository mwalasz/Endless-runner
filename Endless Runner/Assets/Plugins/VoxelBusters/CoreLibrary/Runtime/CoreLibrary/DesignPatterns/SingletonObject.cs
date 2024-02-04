using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class SingletonObject<T> where T : class
    {
        #region Static fields

        [ClearOnReload]
        private     static      T       s_sharedInstance    = null;

        #endregion

        #region Static properties

        public static T Instance => ObjectHelper.CreateInstanceIfNull(
            ref s_sharedInstance,
            CreateInstance);

        #endregion

        #region Private static methods

        private static T CreateInstance()
        {
            // get non-public constructors
            var     ctors   = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            if (!Array.Exists(ctors, (ci) => ci.GetParameters().Length == 0))
            {
                throw new VBException("Non-public ctor() note found.");
            }

            // get reference to default non-public constructor.
            var     ctor    = Array.Find(ctors, (ci) => ci.GetParameters().Length == 0);

            // invoke constructor and return resulting object.
            return ctor.Invoke(new object[] {}) as T;
        }

        #endregion
    }
}