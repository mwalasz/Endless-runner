using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class AsyncOperationHandle<T> : IAsyncOperationHandle<T>, IEquatable<AsyncOperationHandle<T>>
    {
        #region Events

        private event Callback<IAsyncOperationHandle> OnProgressTypeless;

        private event Callback<IAsyncOperationHandle> OnCompleteTypeless;

        #endregion

        #region IAsyncOperationHandle implementation

        public IAsyncOperation<T> InternalOp { get; private set; }

        public AsyncOperationStatus Status => InternalOp.Status;

        public bool IsDone => InternalOp.IsDone;

        public float Progress => InternalOp.Progress;

        object IAsyncOperationHandle.Result => Result;

        public Error Error => InternalOp.Error;

        public T Result => InternalOp.Result;

        event Callback<IAsyncOperationHandle> IAsyncOperationHandle.OnProgress
        {
            add { OnProgressTypeless += value; }
            remove { OnProgressTypeless -= value; }
        }

        event Callback<IAsyncOperationHandle> IAsyncOperationHandle.OnComplete
        {
            add { OnCompleteTypeless += value; }
            remove { OnCompleteTypeless -= value; }
        }

        public event Callback<IAsyncOperationHandle<T>> OnProgress;

        public event Callback<IAsyncOperationHandle<T>> OnComplete;

        #endregion

        #region Constructors

        public AsyncOperationHandle(IAsyncOperation<T> op)
        {
            Assert.IsArgNotNull(op, nameof(op));

            // Set properties
            InternalOp      = op;
            OnProgress      = null;
            OnComplete      = null;
            RegisterForCallbacks();

            // Manually invoke the events incase if the operation is already completed
            if (op.IsDone)
            {
                SurrogateCoroutine.WaitForEndOfFrameAndInvoke(action: () =>
                                                              {
                                                                  HandleOnProgress(op);
                                                                  HandleOnComplete(op);
                                                              });
            }
        }

        #endregion

        #region Private methods

        private void RegisterForCallbacks()
        {
            if (InternalOp == null) return;

            InternalOp.OnProgress += HandleOnProgress;
            InternalOp.OnComplete += HandleOnComplete;
        }

        private void UnregisterCallbacks()
        {
            if (InternalOp == null) return;

            InternalOp.OnProgress -= HandleOnProgress;
            InternalOp.OnComplete -= HandleOnComplete;
        }

        #endregion

        #region Event handler methods

        private void HandleOnProgress(IAsyncOperation<T> asyncOperation)
        {
            // Forward event callback
            OnProgressTypeless?.Invoke(this);
            OnProgress?.Invoke(this);
        }

        private void HandleOnComplete(IAsyncOperation<T> asyncOperation)
        {
            // Forward event callback
            OnCompleteTypeless?.Invoke(this);
            OnComplete?.Invoke(this);

            UnregisterCallbacks();
        }

        #endregion

        #region IEnumerator implementation

        public object Current => null;

        public bool MoveNext() => !InternalOp.IsDone;

	    public void Reset() => InternalOp?.Reset();

        #endregion

        #region IEquatable implemetation

        public bool Equals(AsyncOperationHandle<T> other)
        {
            if ((other == null) || (other.InternalOp == null)) return false;

            if (InternalOp == null) return false;

            return (InternalOp == other.InternalOp);
        }

        #endregion
    }
}