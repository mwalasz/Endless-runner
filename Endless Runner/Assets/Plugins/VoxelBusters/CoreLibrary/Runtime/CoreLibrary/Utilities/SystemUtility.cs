using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

namespace VoxelBusters.CoreLibrary
{
    public static class SystemUtility
    {
        #region String methods

        public static string EscapeString(string value)
        {
            return UnityWebRequest.EscapeURL(value).Replace("+", "%20");
        }

        #endregion

        #region List methods

        public static object[] ConvertListToArray(IList list)
        {
            int     count   = list.Count;
            var     array   = new object[count];
            for (int iter = 0; iter < count; iter++)
            {
                array[iter] = list[iter];
            }

            return array;
        }

        public static TOutput[] ConvertEnumeratorItems<TInput, TOutput>(IEnumerator<TInput> enumerator, Converter<TInput, TOutput> converter, bool includeNullObjects) 
        {
            Assert.IsNotNull(enumerator, "Enumerator is null.");
           
            // create original data array from native data
            var     outputObjects           = new List<TOutput>(capacity: 8);
            while (enumerator.MoveNext())
            {
                var     inputObject         = enumerator.Current;
                var     outputObject        = converter(inputObject);
                if (EqualityComparer<TOutput>.Default.Equals(outputObject, default(TOutput)) && !includeNullObjects)
                {
                    DebugLogger.LogWarning(CoreLibraryDomain.Default, $"Failed to convert object with data {inputObject}.");
                    continue;
                }

                // add object to list
                outputObjects.Add(outputObject);
            }

            return outputObjects.ToArray();
        }

        #endregion
    }
}