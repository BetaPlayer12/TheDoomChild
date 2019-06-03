using UnityEngine;

namespace DChildEditor.Toolkit
{

    public class Commands
    {
        public static GameObject CreateGameObject(string name, Transform parent, Vector3 localPosition)
        {
            var instance = new GameObject(name);
            instance.transform.parent = parent;
            instance.transform.localPosition = localPosition;
            return instance;
        }

        public static GameObject GetOrCreateChildGameObject(string name,Transform parent)
        {
            var combatGO = Commands.FindChildOf(parent, name);
            if (combatGO == null)
            {
                return Commands.CreateGameObject(name, parent, Vector3.zero);
            }
            return combatGO.gameObject;
        }

        public static void SetLayer(GameObject gameObject, string layer, bool includeChildren = false)
        {
            var layerMask = LayerMask.NameToLayer(layer);
            if (includeChildren)
            {
                var gameObjects = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i].gameObject.layer = layerMask;
                }
            }
            else
            {
                gameObject.layer = layerMask;
            }
        }

        public static T GetOrCreateComponent<T>(GameObject gameObject) where T : MonoBehaviour
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static Transform FindChildOf(Transform parent, string childName)
        {
            var children = parent.GetComponentsInChildren<Transform>();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name == childName)
                {
                    return children[i];
                }
            }
            return null;
        }
    }
}