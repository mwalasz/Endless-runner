using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
	public abstract class PrivateSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
        #region Static fields

        [ClearOnReload]
        private     static          T           s_sharedInstance        = null;

        [ClearOnReload(ClearOnReloadOption.Default)]
        private     static readonly object      s_objectLock            = new object();
        
        [ClearOnReload(customValue: false)]
        private     static          bool        s_isDestroyed           = false;

        #endregion

        #region Fields

        [SerializeField]
        private     bool                        m_isPersistent          = true;   

        private     bool                        m_isInitialised         = false;

        private     bool                        m_forcedDestroy         = false;

        #endregion

        #region Static properties

        public static bool IsSingletonActive => (s_sharedInstance != null);

        #endregion

        #region Static methods

        protected static T GetSingleton()
        {
            var     objectType  = typeof(T);
            if (s_isDestroyed) 
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, $"{objectType} instance is already destroyed.");
                return null;
            }

            lock (s_objectLock)
            {
                if (s_sharedInstance == null)
                {
                    // find all the instances that exist in the screen
                    var     sceneInstances  = FindObjectsOfType(objectType) as T[];
                    if (sceneInstances.Length > 0)
                    {
                        // save first element and remove others
                        s_sharedInstance        = sceneInstances[0];
                        for (int iter = 1; iter < sceneInstances.Length; iter++)
                        {
                            Destroy(sceneInstances[iter].gameObject);
                        }
                    }
                    // create new instance
                    else if (s_sharedInstance == null)
                    {
                        string  singletonName   = objectType.Name;
                        s_sharedInstance        = new GameObject(singletonName).AddComponent<T>();
                    }
                }
            }

            // make sure object passed is initialised
            var     instance    = (PrivateSingletonBehaviour<T>)(object)s_sharedInstance;
            if (!instance.m_isInitialised)
            {
                instance.Init();
            }

            return s_sharedInstance;
        }

        protected static bool TryGetSingleton(out T singleton)
        {
            singleton    = GetSingleton();

            return (singleton != null);
        }

        #endregion

        #region Unity methods

        private void Awake()
        {
            if (!m_isInitialised)
            {
                Init();
            }
        }

        private void Start()
        {
            if (s_sharedInstance == this)
            {
                OnSingletonStart();
            }
        }

        private void OnDestroy()
        {
            if (s_sharedInstance == this)
            {
                s_sharedInstance    = null;
                s_isDestroyed       = !m_forcedDestroy;
                OnSingletonDestroy();
            }
        }

        #endregion

        #region Lifecycle methods

        protected virtual void OnSingletonAwake()
        { }

        protected virtual void OnSingletonStart()
        { }

        protected virtual void OnSingletonDestroy()
        { }

        #endregion

        #region Private methods

        private void Init()
        {
            // update flag indicating that instance is initialised
            m_isInitialised         = true;

            // resolve singleton reference
            if (s_sharedInstance == null)
            {
                s_sharedInstance    = this as T;
            }
            else if (s_sharedInstance != this)
            {
                Destroy(gameObject);
                return;
            }

            // invoke internal awake method
            OnSingletonAwake();

            // mark object as persistent, if specified
            if (m_isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion

        #region ISingletonBehaviour implementation

        public bool IsPersistent
        {
            get
            {
                return m_isPersistent;
            }
            set
            {
                m_isPersistent  = value;
            }
        }

        public void DestorySingleton(bool immediate = true)
        {
            if (s_sharedInstance == this)
            {
                m_forcedDestroy = true;
                if (immediate)
                {
                    DestroyImmediate(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        #endregion
    }
}