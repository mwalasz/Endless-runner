using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class EditorWindowZ : EditorWindow
    {
        #region Fields

        [System.NonSerialized]
        private     bool    m_isInitialized;

        #endregion

        #region Unity methods

        protected virtual void OnGUI()
        {
            EnsureInitialized();
        }

        #endregion

        #region Private methods

        protected virtual void Init()
        { }

        protected void EnsureInitialized()
        {
            if (m_isInitialized) return;

            m_isInitialized = true;
            Init();
        }

        #endregion
    }
}