using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class EventHandlerCollection<T> where T : IEventHandler
    {
        #region Fields

        private     List<T>     m_handlers      = new List<T>();

        #endregion

        #region Delegates

        public delegate void EventFunction(T item);

        #endregion

        #region Public methods

        public int IndexOf(T obj)
        {
            // precondition
            Assert.IsArgNotNull(obj, nameof(obj));

            for (int iter = 0; iter < m_handlers.Count; iter++)
            {
                if (EqualityComparer<T>.Default.Equals(m_handlers[iter], obj))
                {
                    return iter;
                }
            }
            return -1;
        }

        public bool Contains(T obj)
        {
            return (-1 != IndexOf(obj));
        }

        public bool Add(T obj)
        {
            // precondition
            Assert.IsArgNotNull(obj, nameof(obj));

            // add object
            if (!Contains(obj))
            {
                int     index   = m_handlers.BinarySearch(obj);
                if (index < 0)
                {
                    m_handlers.Insert(~index, obj);
                }
                else
                {
                    m_handlers.Insert(index, obj);
                }

                return true;
            }

            return false;
        }

        public bool Remove(T handler)
        {
            // precondition
            Assert.IsArgNotNull(handler, nameof(handler));

            // remove object
            int     index   = IndexOf(handler);
            if (index != -1)
            {
                m_handlers.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void SendEvent(EventFunction function)
        {
            // precondition
            Assert.IsArgNotNull(function, nameof(function));

            // invoke action on every compatible object
            var     array   = m_handlers.ToArray();
            int     count   = m_handlers.Count;
            for (int iter = 0; iter < count; iter++)
            {
                try
                {
                    var     current = array[iter];
                    if (current != null)
                    {
                        function(current);
                    }
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(CoreLibraryDomain.Default, exception);
                }
            }
        }

        #endregion

        #region Nested types

        private class EventHandlerComparer : IComparer<T>
        {
            #region IComparer implementation

            public int Compare(T x, T y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (y == null)
                    {
                        return 1;
                    }
                    else
                    {
                        return x.CallbackOrder.CompareTo(y.CallbackOrder);
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}