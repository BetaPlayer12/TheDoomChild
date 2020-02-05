#if UNITY_EDITOR
using UnityEngine;
using DChild.Serialization;
using UnityEngine.SceneManagement;

namespace DChildDebug.Serialization
{
    [ExecuteInEditMode]
    public class ZoneDataDefaultSerializer : MonoBehaviour
    {
        [SerializeField]
        private ZoneDataFile m_file;
        [SerializeField]
        private ZoneDataHandle m_handle;

        private void Awake()
        {
            Debug.Log("Start");
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        private void OnDestroy()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved -= OnSceneSaved;
        }

        private void OnSceneSaved(Scene scene)
        {
            var info = m_handle.GetDefaultData();
            m_file.Set(info.ID, info.data);
        }
    }
} 
#endif