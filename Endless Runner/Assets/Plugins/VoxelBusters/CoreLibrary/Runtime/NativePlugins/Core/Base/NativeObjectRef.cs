using System;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
	public class NativeObjectRef : IDisposable
    {
        #region Fields

        private     bool    m_disposed;

        #endregion

        #region Properties

        public IntPtr Pointer { get; private set; }

        #endregion

        #region Constructors

        public NativeObjectRef(IntPtr ptr, bool autoRetain)
		{
            // check argument valuie
            Assert.IsFalse(ptr == IntPtr.Zero, "Ptr is null.");

            // set properties
            Pointer     = ptr;
			m_disposed  = false;

            if (autoRetain)
            {
                Retain();
            }
		}

        ~NativeObjectRef()
        {
            Dispose(false);
        }

        #endregion

        #region Memory management methods

        private void Retain()
        { 
            RetainInternal(Pointer);
        }

        private void Release()
        { 
            ReleaseInternal(Pointer);
        }

        protected virtual void RetainInternal(IntPtr ptr)
        { }

        protected virtual void ReleaseInternal(IntPtr ptr)
        { }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                DebugLogger.Log(CoreLibraryDomain.NativePlugins, "Disposing native object.");
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects)
                Release();
                m_disposed = true;
            }
        }

        #endregion
    }
}
