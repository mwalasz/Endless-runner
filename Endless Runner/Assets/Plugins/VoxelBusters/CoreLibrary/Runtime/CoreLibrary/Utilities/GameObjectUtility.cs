using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class GameObjectUtility
    {
        #region Static methods

        public static GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            var     newGO   = Object.Instantiate(prefab, parent: parent);
            newGO.name      = prefab.name;

            return newGO;
        }

        public static GameObject Instantiate(GameObject prefab, System.Action<GameObject> onBeforeAwake)
        {
            var     tempGO      = new GameObject("Temp");
            tempGO.SetActive(false);

            var     newInstance = Object.Instantiate(prefab, tempGO.transform);
            newInstance.name    = prefab.name;
            
            try
            {
                // call the onBeforeAwake method
                onBeforeAwake(newInstance);

                return newInstance;
            }
            finally
            {
                // reset gameobject state
                newInstance.transform.SetParent(null);

                // destroy temp object
                Object.Destroy(tempGO);
            }
        }

        public static GameObject CreateChild(string childName, Transform parent)
        {
            return CreateChild(childName, Vector3.zero, Quaternion.identity, Vector3.one, parent);
        }

        public static GameObject CreateChild(string childName, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Transform parent)
        {
            var     containerGO     = (parent is RectTransform) ? new GameObject(childName, typeof(RectTransform)) : new GameObject(childName);

            // set transform properties
            var     containerTrans          = containerGO.transform;
            containerTrans.SetParent(parent);
            containerTrans.localPosition    = localPosition;
            containerTrans.localRotation    = localRotation;
            containerTrans.localScale       = localScale;

            return containerGO;
        }

        public static GameObject CreateGameObject(string name, System.Action<GameObject> onBeforeActive = null)
        {
            var     gameObject      = new GameObject(name);
            try
            {
                if (onBeforeActive != null)
                {
                    gameObject.SetActive(false);
                    onBeforeActive(gameObject);
                }
            }
            finally
            {
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
            }
            return gameObject;
        }

        public static T CreateGameObjectWithComponent<T>(string name, System.Action<T> onBeforeAwake = null) where T : Component
        {
            var     gameObject      = new GameObject(name);
            T       component       = default;
            try
            {
                gameObject.SetActive(false);
                component   = gameObject.AddComponent<T>();
                onBeforeAwake?.Invoke(component);
            }
            finally
            {
                gameObject.SetActive(true);
            }
            return component;
        }

        public static void SetActive(this GameObject[] gameObjects, bool value)
        {
            for (int iter = 0; iter < gameObjects.Length; iter++)
            {
                gameObjects[iter].SetActive(value);
            }
        }

        #endregion
    }
}