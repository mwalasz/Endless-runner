using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class AsyncOperationResultContainer<TData, TError> : IAsyncOperationResultContainer, IAsyncOperationResultContainer<TData, TError> where TError : Error
    {
        #region Fields

        private     TData       m_data;

        private     TError      m_error;

        #endregion

        #region Constructors

        public AsyncOperationResultContainer()
        {
            // set properties
            m_data      = default(TData);
            m_error     = default(TError);
        }

        #endregion

        #region Setter methods

        protected void SetDataInternal(TData data)
        {
            // set value
            m_data      = data;
        }

        protected void SetErrorInternal(TError error)
        {
            // set value
            m_error     = error;
        }

        #endregion

        #region IAsyncOperationResultContainer implementation

        public bool IsError()
        {
            return (m_error != null);
        }

        public string GetErrorDescription()
        {
            return IsError() ? m_error.Description : null; 
        }

        Error IAsyncOperationResultContainer.GetError()
        {
            return GetError();
        }

        object IAsyncOperationResultContainer.GetData()
        {
            return GetData();
        }

        public string GetDataAsText()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Generic IAsyncOperationResultContainer implementation

        public TError GetError()
        {
            return m_error;
        }

        public TData GetData()
        {
            return m_data;
        }

        #endregion
    }
}