using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public interface IAsyncOperation : IEnumerator
    {
        #region Properties

        AsyncOperationStatus Status { get; }

        bool IsDone { get; }

        object Result { get; }

        Error Error { get; }

        float Progress { get; }

        #endregion

        #region Methods

        void Start();

        void Abort();

        #endregion

        #region Events

        event Callback<IAsyncOperation> OnProgress;

        event Callback<IAsyncOperation> OnComplete;

        #endregion
    }

    public interface IAsyncOperation<T> : IAsyncOperation
    {
        #region Properties

        new T Result { get; }

        #endregion

        #region Events

        new event Callback<IAsyncOperation<T>> OnProgress;

        new event Callback<IAsyncOperation<T>> OnComplete;

        #endregion
    }

    public interface IAsyncOperationUpdateHandler
    {
        void Update();
    }
}