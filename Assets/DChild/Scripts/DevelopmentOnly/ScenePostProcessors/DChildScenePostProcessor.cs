#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DChildEditor
{
    public static class DChildScenePostProcessor
    {
        [PostProcessScene(1)]
        private static void OnPostProcessScene()
        {
            var manager = Object.FindObjectOfType<ScenePostProcessorManager>();
            if (manager != null)
            {
                manager.ProcessScene();
                Object.DestroyImmediate(manager);
            }
        }
    }

}
#endif