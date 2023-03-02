using UnityEngine;
using DChild;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

namespace DChildDebug
{
    public class EditorOnlyObject : MonoBehaviour
    {
        [SerializeField]
        private Transform someTransform;

#if UNITY_EDITOR
        [PostProcessScene(0)]
        public static void DeleteOnInstancesOnBuild()
        {
            var instances = FindObjectsOfType<EditorOnlyObject>();
            if (instances.Length > 0)
            {
                var sceneName = instances[0].gameObject.scene.name;
                for (int i = 0; i < instances.Length; i++)
                {
                    instances[i].DeleteInstance();
                }
                CustomDebug.Log(CustomDebug.LogType.Build_PostProcessScene, $"EditorOnlyObject Executed on {sceneName}");
            }
        }

        public void DeleteInstance()
        {
            if (gameObject.scene == SceneManager.GetActiveScene())
                return;

            if (someTransform)
            {
                Object.DestroyImmediate(someTransform);
                Object.DestroyImmediate(this);
            }
            else
            {
                Object.DestroyImmediate(gameObject);
            }
        }
#endif

        //private void OnValidate()
        //{
        //    if (someTransform == null)
        //    {

        //    }
        //    else
        //    {
        //        foreach (Transform tr in someTransform)
        //        {
        //            if (tr.tag != "EditorOnly")
        //            {
        //                tr.tag = "EditorOnly";
        //            }
        //        }
        //    }

        //    //Transform.childCount;
        //    //transform.GetChild()
        //    //var allChildren = GetComponentsInChildren<Transform>();
        //    //foreach (var child in allChildren)
        //    //{
        //    // if(child.tag != "EditorOnly")
        //    // {
        //    // child.tag = "EditorOnly";
        //    //}
        //    //}
        //}
    }
}