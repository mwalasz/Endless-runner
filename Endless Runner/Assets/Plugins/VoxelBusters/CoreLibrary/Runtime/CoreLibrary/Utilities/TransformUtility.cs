using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class TransformUtility 
    {
        #region Extension methods

        public static Transform[] GetImmediateChildren(Transform transform)
        {
            int     childCount  = transform.childCount;
            var     children    = new Transform[childCount];
            for (int iter = 0; iter < childCount; iter++)
            {
                children[iter]  = transform.GetChild(iter);
            }
            return children;
        }

        public static T FindComponentInChildren<T>(GameObject gameObject, string name)
        {
            return gameObject.transform.Find(name).GetComponent<T>();
        }

        public static void RemoveAllChilds(this Transform parent)
        {
            RemoveChildren(parent);
        }

        public static void RemoveChildren(this Transform parent)
        {
            var     children    = GetImmediateChildren(parent);
            for (int iter = 0; iter < children.Length; iter++)
            {
                Object.Destroy(children[iter].gameObject);
            }
        }

        public static bool RemoveChild(this Transform parent, int index)
        {
            if ((index < 0) || (index >= parent.childCount)) return false;

            Object.Destroy(parent.GetChild(index).gameObject);
            return true;
        }

        #endregion
    }
}