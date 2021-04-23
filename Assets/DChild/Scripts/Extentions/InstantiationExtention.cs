using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild
{
    public static class InstantiationExtention
    {
        public static GameObject InstantiateToScene(this MonoBehaviour mono, GameObject gameObject)
        {
            var instance = Object.Instantiate(gameObject);
            SceneManager.MoveGameObjectToScene(instance, mono.gameObject.scene);
            return instance;
        }

        public static GameObject InstantiateToScene(this MonoBehaviour mono, GameObject gameObject, Transform parent)
        {
            var instance = Object.Instantiate(gameObject, parent);
            return instance;
        }

        public static GameObject InstantiateToScene(this MonoBehaviour mono, GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            var instance = Object.Instantiate(gameObject, position, rotation);
            SceneManager.MoveGameObjectToScene(instance, mono.gameObject.scene);
            return instance;
        }

        public static GameObject InstantiateToScene(this MonoBehaviour mono, GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            var instance = Object.Instantiate(gameObject, position, rotation, parent);
            SceneManager.MoveGameObjectToScene(instance, mono.gameObject.scene);
            return instance;
        }

        public static GameObject InstantiateToScene(this MonoBehaviour mono, GameObject original, Scene scene)
        {
            GameObject instance = Object.Instantiate(original);
            SceneManager.MoveGameObjectToScene(instance, scene);
            return instance;
        }

        public static GameObject InstantiateToScene(this MonoBehaviour mono, GameObject original, Vector3 position, Quaternion rotation, Scene scene)
        {
            GameObject instance = Object.Instantiate(original, position, rotation);
            SceneManager.MoveGameObjectToScene(instance, scene);
            return instance;
        }

        public static GameObject InstantiateToScene(this System.Object @object, GameObject original, Scene scene)
        {
            GameObject instance = Object.Instantiate(original);
            SceneManager.MoveGameObjectToScene(instance, scene);
            return instance;
        }
    }
}
