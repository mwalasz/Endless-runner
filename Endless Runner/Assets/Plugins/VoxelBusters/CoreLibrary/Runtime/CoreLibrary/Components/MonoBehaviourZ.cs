using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class MonoBehaviourZ : MonoBehaviour
    {
        #region Fields

        private     bool    m_isInitialisedInternal     = false;

        #endregion

        #region Unity methods

        private void Awake()
        {
            EnsureInitialised();
        }

        protected virtual void Start()
        { }

        protected virtual void OnEnable()
        { }

        protected virtual void OnDisable()
        { }

        protected virtual void OnDestroy()
        { }

        #endregion

        #region Private methods

        protected void EnsureInitialised()
        {
            if (m_isInitialisedInternal) return;

            m_isInitialisedInternal = true;

            Init();
        }

        protected virtual void Init()
        { }

        #endregion
    }
}