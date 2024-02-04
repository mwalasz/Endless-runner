using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class Scheduler
    {
        #region Static fields

		[ClearOnReload]
        private     static  IScheduler  s_scheduler;

        #endregion

        #region Static events

        public static event Callback Update
        {
            add
            {
                EnsureInitialised();
                s_scheduler.Update  += value;
            }
            remove
            {
                EnsureInitialised();
                s_scheduler.Update  -= value;
            }
        }

        #endregion

        #region Static methods

        public static void StartCoroutine(IEnumerator routine)
        {
            EnsureInitialised();

            s_scheduler.StartCoroutine(routine);
        }

        public static void StopCoroutine(IEnumerator routine)
        {
            EnsureInitialised();

            s_scheduler.StopCoroutine(routine);
        }

        public static void StopAllCoroutines()
        {
            EnsureInitialised();

            s_scheduler.StopAllCoroutines();
        }

        private static void EnsureInitialised()
        {
            if (s_scheduler != null) return;

        #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                s_scheduler = new EditorScheduler();
            }
            else
        #endif
            {
                s_scheduler = RuntimeScheduler.Instance;
            }
        }

        #endregion
    }
}