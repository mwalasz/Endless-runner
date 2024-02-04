using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class GroupOperation : AsyncOperation<object>
    {
        #region Fields

        private     int             m_operationCount;

        private     bool            m_abortOnError;

        #endregion

        #region Properties

        public IAsyncOperation[] Operations { get; private set; }

        #endregion

        #region Constructors

        public GroupOperation(bool abortOnError = false, params IAsyncOperation[] operations)
        {
            Assert.IsArgNotNull(operations, nameof(operations));

            int     count       = operations.Length;
            Assert.IsNotZero(count, "Array is empty.");
            
            // set properties
            Operations          = operations;
            m_operationCount    = count;
            m_abortOnError      = abortOnError;
        }

        #endregion

        #region Base class methods

        public override void Reset()
        {
            base.Reset();

            // reset properties
            for (int iter = 0; iter < m_operationCount; iter++)
            {
                Operations[iter].Reset();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            // start all operations
            for (int iter = 0; iter < m_operationCount; iter++)
            {
                var     current = Operations[iter];
                current.Start();
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // iterate through the list and check current status of participating operations
            bool    isDone      = true;
            Error   error       = null;
            for (int iter = 0; iter < m_operationCount; iter++)
            {
                // check whether operation has completed
                var     current = Operations[iter];
                if (!current.IsDone)
                {
                    isDone      = false;
                    break;
                }

                if (AsyncOperationStatus.Failed == current.Status)
                {
                    if (m_abortOnError)
                    {
                        error   = current.Error;
                        break;
                    }
                }
            }

            // update state based on operation results
            if (isDone)
            {
                if (error != null)
                {
                    if (m_abortOnError)
                    {
                        AbortActiveOperations();
                    }
                    SetIsCompleted(error: error);
                }
                else
                {
                    SetIsCompleted(result: null);
                }
            }
            else
            {
                UpdateProgress();
            }
        }

        protected override void SetIsCompleted(object result)
        {
            // set final progress value
            Progress    = 1f;

            base.SetIsCompleted(result);
        }

        protected override void SetIsCompleted(Error error)
        {
            // set final progress value
            Progress    = 0f;

            base.SetIsCompleted(error);
        }

        #endregion

        #region Private methods

        private void UpdateProgress()
        { 
            // find cumulative progress
            float   progress    = 0f;
            for (int iter = 0; iter < m_operationCount; iter++)
            {
                progress       += Operations[iter].Progress;
            }

            // set normalized value
            Progress    = (progress / m_operationCount);
        }

        private void AbortActiveOperations()
        {
            for (int iter = 0; iter < m_operationCount; iter++)
            {
                var     current = Operations[iter];
                if (!current.IsDone)
                {
                    current.Abort();
                }
            }
        }

        #endregion
    }
}