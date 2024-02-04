using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public interface IAsyncOperationResultContainer
    {
        #region Methods

        bool IsError();

        string GetErrorDescription();

        Error GetError();

        object GetData();

        string GetDataAsText();

        #endregion
    }

    public interface IAsyncOperationResultContainer<TData, TError> : IAsyncOperationResultContainer where TError : Error
    {
        #region Methods

        new TError GetError();

        new TData GetData();

        #endregion
    }
}