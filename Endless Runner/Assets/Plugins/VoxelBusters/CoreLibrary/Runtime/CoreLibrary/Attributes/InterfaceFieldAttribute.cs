using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class InterfaceFieldAttribute : PropertyAttribute
    {
        #region Properties

        public Type InterfaceType
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public InterfaceFieldAttribute(Type interfaceType)
        {
            // set properties
            InterfaceType   = interfaceType;
        }

        #endregion
    }
}