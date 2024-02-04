#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public abstract class NativeAndroidJavaObjectWrapper
    {
        protected AndroidJavaObject m_nativeObject;
        protected string m_className;

        public AndroidJavaObject NativeObject
        {
            get
            {
                return m_nativeObject;
            }
        }

        public NativeAndroidJavaObjectWrapper(string className, params object[] args)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log(string.Format("Creating {0}", this.GetType()));
#endif
            m_className         = className;
            m_nativeObject      = new AndroidJavaObject(className, args);
        }

        public NativeAndroidJavaObjectWrapper(string className, AndroidJavaObject androidJavaObject)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log(string.Format("Creating from AndroidJavaObject : {0}", this.GetType()));
#endif
            m_className     = className;
            m_nativeObject  = androidJavaObject;
        }

        public NativeAndroidJavaObjectWrapper(NativeAndroidJavaObjectWrapper wrapper)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log(string.Format("Creating from wrapper : {0}", this.GetType()));
#endif
            m_className     = wrapper.GetClassName();
            m_nativeObject  = wrapper.NativeObject;
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeAndroidJavaObjectWrapper()
        {
            //DebugLogger.Log("Destructor for " + this.GetType());
        }
#endif

        public static AndroidJavaObject CreateFromStatic(string className, string methodName, params object[] args)
        {
            AndroidJavaClass cls = new AndroidJavaClass(className);
            AndroidJavaObject androidJavaObject = cls.CallStatic<AndroidJavaObject>(methodName, args);
            return androidJavaObject;
        }

        public bool IsNull()
        {
            return m_nativeObject == null;
        }

        protected T Get<T>(string fieldName)
        {
            return m_nativeObject.Get<T>(fieldName);
        }

        protected void Set<T>(string fieldName, T val)
        {
            m_nativeObject.Set(fieldName, val);
        }

        protected void Call(string methodName, params object[] args)
        {
            if(!IsNull())
            {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                Debug.Log(string.Format("[Class : {0}] [Method : {1}]", this.GetType(), methodName));
#endif
                m_nativeObject.Call(methodName, args);
            }
            else
            {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                Debug.Log(string.Format("Null Call [Class : {0}] [Method : {1}]", this.GetType(), methodName));
#endif
            }
        }

        protected T Call<T>(string methodName, params object[] args)
        {
            if(!IsNull())
            {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                Debug.Log(string.Format("[Class : {0}] [Method : {1}]", this.GetType(), methodName));
#endif
                return m_nativeObject.Call<T>(methodName, args);
            }
            else
            {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                Debug.Log(string.Format("Null Call [Class : {0}] [Method : {1}]", this.GetType(), methodName));
#endif
                return default(T);
            }
        }

        protected string GetClassName()
        {
            return m_className;
        }
    }
}
#endif