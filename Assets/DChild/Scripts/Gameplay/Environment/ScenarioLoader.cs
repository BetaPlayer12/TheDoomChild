using DChild.Serialization;
using Holysoft.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Environment
{
    public class ScenarioLoader : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        private struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_scenarioIsOver;

            public SaveData(bool scenarioIsOver)
            {
                m_scenarioIsOver = scenarioIsOver;
            }

            public bool scenarioIsOver => m_scenarioIsOver;

            public ISaveData ProduceCopy() => new SaveData(m_scenarioIsOver);
        }

        [SerializeField]
        private SceneInfo m_sceneInfo;
        private bool m_scenarioIsOver;
        private bool m_sceneLoaded;

        public void SetScenarioState(bool isOver) => m_scenarioIsOver = isOver;

        public void ValidateCurrentState()
        {
            if (m_scenarioIsOver && m_sceneLoaded)
            {
                SceneManager.UnloadSceneAsync(m_sceneInfo.sceneName);
                m_sceneLoaded = false;
            }
            else if (m_scenarioIsOver == false && m_sceneLoaded == false)
            {
                SceneManager.LoadSceneAsync(m_sceneInfo.sceneName, LoadSceneMode.Additive);
                m_sceneLoaded = true;
            }
        }

        public void Load(ISaveData data)
        {
            m_scenarioIsOver = ((SaveData)data).scenarioIsOver;
            ValidateCurrentState();
        }

        public ISaveData Save()
        {
            return new SaveData(m_scenarioIsOver);
        }

        private void Start()
        {
            if (m_scenarioIsOver == false && m_sceneLoaded == false)
            {
                SceneManager.LoadSceneAsync(m_sceneInfo.sceneName, LoadSceneMode.Additive);
                m_sceneLoaded = true;
            }
        }

        private void OnDestroy()
        {
            if (m_sceneLoaded)
            {
                SceneManager.UnloadSceneAsync(m_sceneInfo.sceneName);
                m_sceneLoaded = false;
            }
        }
    }
}