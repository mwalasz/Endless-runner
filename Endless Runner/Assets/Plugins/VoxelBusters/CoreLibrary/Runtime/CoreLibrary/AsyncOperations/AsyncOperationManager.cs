using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    internal static class AsyncOperationManager
    {
        #region Properties

        [ClearOnReload]
        private     static  List<IAsyncOperationUpdateHandler>  s_targets;

        #endregion

        #region Static methods

        public static void ScheduleUpdate(IAsyncOperationUpdateHandler target)
        {
            Assert.IsArgNotNull(target, nameof(target));

            EnsureInitialised();

            s_targets.Add(target);
        }

        public static void UnscheduleUpdate(IAsyncOperationUpdateHandler target)
        {
            Assert.IsArgNotNull(target, nameof(target));

            EnsureInitialised();

            s_targets.Remove(target);
        }

        #endregion

        #region Unity methods

        private static void Update()
        {
            UpdateTargets();
        }

        #endregion

        #region Private methods

        private static void EnsureInitialised()
        {
            if (s_targets != null) return;

            // Set properties
            s_targets           = new List<IAsyncOperationUpdateHandler>(8);

            // Register for callbacks
            Scheduler.Update   += Update;
        }

        private static void UpdateTargets()
        {
            for (int iter = 0; iter < s_targets.Count; iter++)
            {
                var     target  = s_targets[iter];
                if (target != null)
                {
                    target.Update();
                }
            }
        }
         
        #endregion
    }
}