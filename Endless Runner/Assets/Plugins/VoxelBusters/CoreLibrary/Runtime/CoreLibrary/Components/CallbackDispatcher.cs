using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void Callback();

    /// <summary>
    /// Generic callback definition for events.
    /// </summary>
    public delegate void Callback<TArg>(TArg arg);

    /// <summary>
    /// Generic callback definition for events.
    /// </summary>
    public delegate void SuccessCallback<TResult>(TResult result);

    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void ErrorCallback(Error error);

    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void CompletionCallback(bool success, Error error);

    /// <summary>
    /// Generic callback definition for events.
    /// </summary>
    public delegate void CompletionCallback<TResult>(TResult result, Error error);

    /// <summary>
    /// Generic callback definition for operations.
    /// </summary>
    public delegate void EventCallback<TResult>(TResult result, Error error);

    public delegate void CompletionCallbackLegacy(Error error);

    public class CallbackDispatcher : PrivateSingletonBehaviour<CallbackDispatcher>
    {
        #region Fields

        private     Queue<Action>       m_queue;

        #endregion

        #region Static methods

        internal static CallbackDispatcher Initialize()
        {
            return GetSingleton();
        }

        public static void InvokeOnMainThread(Callback callback)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(action: () => callback.Invoke());
            }
        }

        public static void InvokeOnMainThread<TArg>(Callback<TArg> callback, TArg arg)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(arg));
            }
        }

        public static void InvokeOnMainThread<TResult>(SuccessCallback<TResult> callback, TResult result)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(result));
            }
        }

        public static void InvokeOnMainThread(ErrorCallback callback, Error error)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(error));
            }
        }

        public static void InvokeOnMainThread(CompletionCallback callback, bool success, Error error)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(action: () => callback.Invoke(success, error));
            }
        }

        public static void InvokeOnMainThread<TResult>(CompletionCallback<TResult> callback, TResult result, Error error)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(result, error));
            }
        }

        public static void InvokeOnMainThread<TResult>(EventCallback<TResult> callback, IOperationResultContainer<TResult> resultContainer)
        {
            InvokeOnMainThread(callback, resultContainer.GetResult(), resultContainer.GetError());
        }

        public static void InvokeOnMainThread<TResult>(EventCallback<TResult> callback, TResult result, Error error)
        {
            // validate arguments
            if (callback == null)
            {
                //DebugLogger.LogWarning("Callback is null.");
                return;
            }

            // add request to queue
            var     manager     = GetSingleton();
            if (manager)
            {
                manager.AddAction(() => callback.Invoke(result, error));
            }
        }

        #endregion

        #region Unity methods

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

            // Set properties
            m_queue     = new Queue<Action>(capacity: 16);
        }

        private void LateUpdate()
        {
            try
            {
                // execute pending actions
                while (m_queue.Count > 0)
                {
                    var     action  = m_queue.Dequeue();
                    action();
                }
            }
            catch (Exception expection)
            {
                DebugLogger.LogException(CoreLibraryDomain.Default, expection);

            }
        }

        #endregion

        #region Private methods

        private void AddAction(Action action)
        {
            m_queue.Enqueue(action);
        }

        #endregion
    }
}