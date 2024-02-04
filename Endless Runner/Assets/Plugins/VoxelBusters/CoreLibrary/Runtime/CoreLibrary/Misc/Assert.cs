using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class Assert
    {
        #region Static methods

        public static void IsNotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new VBException(message);
            }
        }

        public static void IsNull(object obj, string message)
        {
            if (obj != null)
            {
                throw new VBException(message);
            }
        }
        
        public static void IsPropertyNotNull(object obj, string property)
        {
            if (obj == null)
            {
                throw new VBException(string.Format("{0} is null.", property));
            }
        }

        public static void IsArgNotNull(object obj, string argName)
        {
            if (obj == null)
            {
                throw new VBException(string.Format("Arg {0} is null.", argName));
            }
        }

        public static void AreNotEqual(object obj1, object obj2, string message)
        {
            if (obj1 == obj2)
            {
                throw new VBException(message);
            }
        }

        public static void AreNotEqual<T>(T value, T target, string message)
        {
            if (EqualityComparer<T>.Default.Equals(value, target))
            {
                throw new VBException(message);
            }
        }

        public static void AreEqual<T>(T value, T target, string message)
        {
            if (!EqualityComparer<T>.Default.Equals(value, target))
            {
                throw new VBException(message);
            }
        }

        public static void IsTrue(bool status, string message)
        {
            if (!status)
            {
                throw new VBException(message);
            }
        }

        public static void IsFalse(bool status, string message)
        {
            if (status)
            {
                throw new VBException(message);
            }
        }

        public static void IsNotZero(int value, string message)
        {
            AreNotEqual(value, 0, message);
        }

        public static void IsNotNullOrEmpty(string value, string message)
        {
            IsFalse(string.IsNullOrEmpty(value), message);
        }

        public static void IsNotNullOrEmpty<T>(T[] array, string name)
        {
            if (array == null)
            {
                throw new VBException(string.Format("{0} is null.", name));
            }
            if (0 == array.Length)
            {
                throw new VBException(string.Format("{0} is empty.", name));
            }
        }

        #endregion
    }
}