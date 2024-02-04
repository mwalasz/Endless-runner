#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    internal class EditorScheduler : IScheduler
    {
        #region Fields

        private     List<IEnumerator>   m_activeCoroutines;

        #endregion

        #region Constructors

        public EditorScheduler()
        {
            // Set properties
            m_activeCoroutines  = new List<IEnumerator>();

            // Register for callback
            UnityEditor.EditorApplication.update    += EditorUpdate;
        }

        ~EditorScheduler()
        {
            // Unregister from callback
            UnityEditor.EditorApplication.update    -= EditorUpdate;
        }

        #endregion

        #region Private methods

        private void EditorUpdate()
        {
            UpdateCoroutines();
            SendUpdateEvent();
        }

        private void UpdateCoroutines()
        {
            for (int iter = 0; iter < m_activeCoroutines.Count; iter++)
            {
                var     routine = m_activeCoroutines[iter];
                if (routine == null) continue;

                if (!routine.MoveNext())
                {
                    m_activeCoroutines.RemoveAt(iter);
                    iter--;
                }
            }
        }

        private void SendUpdateEvent()
        {
            Update?.Invoke();
        }

        #endregion

        #region IScheduler implementation

        public event Callback Update;

        public void StartCoroutine(IEnumerator routine)
        {
            if (routine == null) return;

            m_activeCoroutines.AddUnique(routine);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            if (routine == null) return;

            m_activeCoroutines.Remove(routine);
        }

        public void StopAllCoroutines()
        {
            m_activeCoroutines.Clear();
        }

        #endregion
    }
}
#endif