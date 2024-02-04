using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public abstract class NativeFeatureBehaviour : MonoBehaviour
    {
        #region Fields

        private     bool    m_isInitialised     = false;

        #endregion

        #region Abstract methods

        public abstract bool IsAvailable();

        protected abstract string GetFeatureName();

        #endregion

        #region Static methods

        public static T CreateInstance<T>(string name = "GameObject") where T : NativeFeatureBehaviour
        {
            return CreateInstanceInternal<T>(name, null);
        }

        protected static T CreateInstanceInternal<T>(string name, params object[] args) where T : NativeFeatureBehaviour
        {
            T       instance            = new GameObject(name).AddComponent<T>();
            instance.AwakeInternal(args);

            return instance;
        }

        #endregion

        #region Unity methods

        private void Awake()
        { }

        private void Start()
        { 
            // check feature availablity
            if (!IsAvailable())
            {
                Diagnostics.LogNotSupported(featureName: GetFeatureName());
                return;
            }

            StartInternal();
        }

        protected void OnDestroy()
        { 
            DebugLogger.Log(CoreLibraryDomain.NativePlugins, $"Destroying native feature behaviour: {name}.");
            DestroyInternal();
        }

        #endregion

        #region Lifecycle methods

        protected virtual void AwakeInternal(object[] args)
        {
            // check component status
            Assert.IsFalse(m_isInitialised, "Initialisation error");

            // update state
            m_isInitialised = true; 
        }

        protected virtual void StartInternal()
        { }

        protected virtual void DestroyInternal()
        { }

        #endregion
    }
}