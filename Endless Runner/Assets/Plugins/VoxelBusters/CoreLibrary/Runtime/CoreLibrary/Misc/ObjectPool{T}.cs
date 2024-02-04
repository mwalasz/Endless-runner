// credits: https://bitbucket.org/Unity-Technologies/ui/src/31cbc456efd5ed74cba398ec1a101a31f66716db/UnityEngine.UI/UI/Core/Utility/ObjectPool.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class ObjectPool<T> where T : class
    {
        #region Fields

        private     readonly    Stack<T>            m_stack;

        private     readonly    System.Func<T>      m_createFunc;
        
        private     readonly    Callback<T>         m_actionOnGet;
        
        private     readonly    Callback<T>         m_actionOnAdd;
        
        private     readonly    Callback<T>         m_actionOnRelease;

        #endregion

        #region Properties

        public int CountAll { get; private set; }

        public int CountActive { get { return CountAll - CountInactive; } }

        public int CountInactive { get { return m_stack.Count; } }

        #endregion

        #region Constructors

        public ObjectPool(System.Func<T> createFunc, Callback<T> actionOnGet = null,
            Callback<T> actionOnAdd = null, Callback<T> actionOnRelease = null)
        {
            m_stack             = new Stack<T>(capacity: 16);
            m_createFunc        = createFunc;
            m_actionOnGet       = actionOnGet;
            m_actionOnAdd       = actionOnAdd;
            m_actionOnRelease   = actionOnRelease;
        }

        #endregion

        #region Public methods

        public T Get()
        {
            // remove item
            T   element;
            if (m_stack.Count == 0)
            {
                element = m_createFunc();
                CountAll++;
            }
            else
            {
                element = m_stack.Pop();
            }

            // send event
            m_actionOnGet?.Invoke(element);

            return element;
        }

        public void Add(T element)
        {
            if (m_stack.Count > 0 && ReferenceEquals(m_stack.Peek(), element))
            {
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            }

            // send event
            m_actionOnAdd?.Invoke(element);

            // add item
            m_stack.Push(element);
        }

        public void Reset()
        {
            CountAll    = 0;
            while (m_stack.Count > 0)
            {
                var     item    = m_stack.Pop();
                m_actionOnRelease?.Invoke(item);
            }
        }

        #endregion
    }
}