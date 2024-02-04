using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [System.Serializable]
    public class ReadableId
    {
        #region Fields

        [SerializeField]
        private     string      m_name;

        [SerializeField]
        private     string      m_id;

        #endregion

        #region Properties

        public string Name => m_name;

        public string Id => m_id;

        #endregion

        #region Constructors

        public ReadableId(string name, string id)
        {
            // set properties
            m_name  = name;
            m_id    = id;
        }

        #endregion
    }
}