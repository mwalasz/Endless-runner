using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class ChainedOperation : AsyncOperation<object>
    {
        #region Fields

        private     int             m_operationCount;

        private     bool            m_abortOnError;

        private     IAsyncOperation m_activeOperation;

        private     int             m_activeOperationIndex;
        
        #endregion

        #region Properties

        public IAsyncOperation[] Operations { get; private set; }

        #endregion

        #region Constructors

        public ChainedOperation(bool abortOnError = false, params IAsyncOperation[] operations)
        {
            Assert.IsArgNotNull(operations, nameof(operations));

            int     count           = operations.Length;
            Assert.IsNotZero(count, "Array is empty.");
            
            // set properties
            Operations              = operations;
            m_operationCount        = count;
            m_abortOnError          = abortOnError;
            m_activeOperationIndex  = -1;
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
            m_activeOperation       = null;
            m_activeOperationIndex  = -1;
        }

        protected override void OnStart()
        {
            base.OnStart();

            // start first operation
            StartOperation(index: 0);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // check whether current operation is completed
            if (m_activeOperation == null)
            {
                SetIsCompleted(error: new Error("Unknown error!"));
                return;
            }

            if (m_activeOperation.IsDone)
            {
                if (m_abortOnError && (m_activeOperation.Status == AsyncOperationStatus.Failed))
                {
                    SetIsCompleted(error: m_activeOperation.Error);
                    return;
                }
                // proceed to the next operation
                if (!StartOperation(index: (m_activeOperationIndex + 1)))
                {
                    SetIsCompleted(result: null);
                    return;
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

        private bool StartOperation(int index)
        {
            if (index < m_operationCount)
            {
                m_activeOperation       = Operations[index];
                m_activeOperationIndex  = index;
                m_activeOperation.Start();

                return true;
            }

            return false;
        }

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

        #endregion
    }
}