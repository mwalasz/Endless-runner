using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public interface IScheduler
    {
        #region Events

        event Callback Update;

        #endregion

        #region Methods

        void StartCoroutine(IEnumerator routine);

        void StopCoroutine(IEnumerator routine);

        void StopAllCoroutines();

        #endregion
    }
}