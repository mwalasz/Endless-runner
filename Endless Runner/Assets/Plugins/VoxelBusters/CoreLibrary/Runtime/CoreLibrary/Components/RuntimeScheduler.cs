using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary
{
    internal class RuntimeScheduler : SingletonBehaviour<RuntimeScheduler>, IScheduler
	{
        #region Fields

        private event Callback UpdateEvent;  

        #endregion

        #region Unity methods

        private void Update()
        {
            SendUpdateEvent();
        }

        #endregion

        #region Private methods

        private void SendUpdateEvent()
        {
            UpdateEvent?.Invoke();
        }

        #endregion

        #region IScheduler implementation

        event Callback IScheduler.Update
        {
            add { UpdateEvent += value; }
            remove { UpdateEvent -= value; }
        }

        void IScheduler.StartCoroutine(IEnumerator routine)
        {
            StartCoroutine(routine);
        }

        void IScheduler.StopCoroutine(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        void IScheduler.StopAllCoroutines()
        {
            StopAllCoroutines();
        }

        #endregion
    }
}