using Holysoft.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild
{
    [System.Serializable]
    public struct GameIntroHandler
    {
        [SerializeField]
        private SceneInfo m_introScene;
        [SerializeField]
        private bool m_hasPlayedIntro;

        public void Execute()
        {
            if (m_hasPlayedIntro == false)
            {
                SceneManager.LoadSceneAsync(m_introScene.sceneName, LoadSceneMode.Additive);
                m_hasPlayedIntro = true;
            }
        }
    }
}