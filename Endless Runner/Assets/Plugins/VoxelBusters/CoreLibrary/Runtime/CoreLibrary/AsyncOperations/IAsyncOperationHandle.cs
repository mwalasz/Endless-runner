using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public interface IAsyncOperationHandle : IEnumerator
    {
        #region Properties

        AsyncOperationStatus Status { get; }

        bool IsDone { get; }

        object Result { get; }

        Error Error { get; }

        float Progress { get; }

        #endregion

        #region Events

        event Callback<IAsyncOperationHandle> OnProgress;

        event Callback<IAsyncOperationHandle> OnComplete;

        #endregion
    }

    public interface IAsyncOperationHandle<T> : IAsyncOperationHandle
    {
        #region Properties

        new T Result { get; }

        #endregion

        #region Events

        new event Callback<IAsyncOperationHandle<T>> OnProgress;

        new event Callback<IAsyncOperationHandle<T>> OnComplete;

        #endregion
    }
}