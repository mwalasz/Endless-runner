using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
    public class DemoResources : PrivateSingletonBehaviour<DemoResources>
    {
        #region Fields

        [SerializeField]
        private     Texture2D[]         m_images    = null;

        [SerializeField]
        private     string[]            m_urls      = null;

        [SerializeField]
        private     string[]            m_texts     = null;

        #endregion

        #region Public methods

        public static Texture2D GetRandomImage()
        { 
            return (Texture2D)GetRandomItem(GetSingleton().m_images);
        }

        public static string GetRandomURL()
        { 
            return (string)GetRandomItem(GetSingleton().m_urls);
        }

        public static string GetRandomText()
        { 
            return (string)GetRandomItem(GetSingleton().m_texts);
        }

        #endregion

        #region Private static methods

        private static object GetRandomItem(Array array)
        {
            int randomIndex = UnityEngine.Random.Range(0, array.Length);
            return array.GetValue(randomIndex);
        }

        #endregion
    }
}