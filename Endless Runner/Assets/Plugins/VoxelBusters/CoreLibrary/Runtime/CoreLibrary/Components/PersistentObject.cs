using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class PersistentObject : MonoBehaviour 
    {
        #region Unity methods

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}