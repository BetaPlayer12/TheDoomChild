#if UNITY_EDITOR
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChildDebug
{
    public class RequiredSceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneInfo[] m_scenes;
        [SerializeField]
        private SceneInfo m_activeScene;

        [Button("Load Scenes")]
        private void LoadScenes()
        {
            for (int i = 0; i < m_scenes.Length; i++)
            {
                if (EditorSceneManager.GetSceneByName(m_scenes[i].sceneName).isLoaded == false)
                {
                    EditorSceneManager.OpenScene(m_scenes[i].scenePath, OpenSceneMode.Additive);
                }
            }
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName(m_activeScene.sceneName));
        }
    }
}
#endif