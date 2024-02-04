using System.Collections;

namespace VoxelBusters.CoreLibrary
{
    public abstract class AsyncOperation<T> : IAsyncOperation, IAsyncOperation<T>, IAsyncOperationUpdateHandler
    {
        #region Events

        private event Callback<IAsyncOperation> OnProgressTypeless;

        private event Callback<IAsyncOperation> OnCompleteTypeless;

        #endregion

        #region Constructors

        protected AsyncOperation()
        {
            // Set initial values
            Status          = AsyncOperationStatus.NotStarted;
            IsDone          = false;
            Error           = null;
            OnComplete      = null;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            // Check whether operation is already started
            if (!IsCurrentStatus(AsyncOperationStatus.NotStarted))
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, "The requested operation could not be started.");
                return;
            }

            SetStarted();
        }

        public void Abort()
        {
            // Check whether operation is already completed
            if (!IsCurrentStatus(AsyncOperationStatus.InProgress))
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, "The requested operation could not be cancelled.");
                return;
            }

            OnAbort();
            SetIsCompleted(error: new Error("Async operation was cancelled!"));
        }

        #endregion

        #region Private methods

        private bool IsCurrentStatus(AsyncOperationStatus status)
        {
            return (Status == status);
        }

        private void SetStarted()
        {
            // Update instance state
            Status      = AsyncOperationStatus.InProgress;

            // Register instance to the scheduler
            AsyncOperationManager.ScheduleUpdate(this);

            // Send state specific message
            OnStart();
        }

        protected virtual void SetIsCompleted(T result = default(T))
        {
            SetIsCompletedInternal(
                result: result,
                error: null,
                status: AsyncOperationStatus.Succeeded);
        }

        protected virtual void SetIsCompleted(Error error)
        {
            Assert.IsArgNotNull(error, nameof(error));
            
            SetIsCompletedInternal(
                result: default(T),
                error: error,
                status: AsyncOperationStatus.Failed);
        }

        private void SetIsCompletedInternal(T result, Error error, AsyncOperationStatus status)
        {
            // Check whether status can be updated
            if (!IsCurrentStatus(AsyncOperationStatus.InProgress))
            {
                DebugLogger.LogWarning(CoreLibraryDomain.Default, "The requested operation could not be marked as completed.");
                return;
            }

            // Unregister object from scheduler
            AsyncOperationManager.UnscheduleUpdate(this);

            // Update instance state
            IsDone      = true;
            Result      = result;
            Error       = error;
            Status      = status;

            // Send state specific message
            OnEnd();

            SendCompleteEvent();
        }

        private void SendProgressEvent()
        {
            OnProgressTypeless?.Invoke(this);
            OnProgress?.Invoke(this);
        }

        private void SendCompleteEvent()
        {
            OnCompleteTypeless?.Invoke(this);
            OnComplete?.Invoke(this);
        }

        #endregion

        #region State messages
        
        protected virtual void OnStart()
        { }

        protected virtual void OnUpdate()
        { }

        protected virtual void OnEnd()
        { }

        protected virtual void OnAbort()
        { }

        #endregion

        #region IAsyncOperation implementation

        public AsyncOperationStatus Status { get; private set; }

        public bool IsDone { get; private set; }

        object IAsyncOperation.Result => Result;

        public Error Error { get; private set; }

        public float Progress { get; protected set; }

        event Callback<IAsyncOperation> IAsyncOperation.OnProgress
        {
            add { OnProgressTypeless += value; }
            remove { OnProgressTypeless -= value; }
        }

        event Callback<IAsyncOperation> IAsyncOperation.OnComplete
        {
            add { OnCompleteTypeless += value; }
            remove { OnCompleteTypeless -= value; }
        }

        public T Result { get; private set; }

        public event Callback<IAsyncOperation<T>> OnProgress;

        public event Callback<IAsyncOperation<T>> OnComplete;

        object IEnumerator.Current => null;

        bool IEnumerator.MoveNext()
        {
            if (IsCurrentStatus(AsyncOperationStatus.NotStarted))
            {
                SetStarted();
            }
            
            return !IsDone;
        }

        public virtual void Reset()
        {
            // Reset properties
            Status      = AsyncOperationStatus.NotStarted;
            IsDone      = false;
            Error       = null;
            Progress    = 0f;
        }

        #endregion

        #region IAsyncOperationUpdateHandler implemetation

        void IAsyncOperationUpdateHandler.Update()
        {
            // Execute start method instructions when operation begins
            if (IsCurrentStatus(AsyncOperationStatus.InProgress))
            {
                OnUpdate();
                SendProgressEvent();
                return;
            }
        }

        #endregion
    }
}