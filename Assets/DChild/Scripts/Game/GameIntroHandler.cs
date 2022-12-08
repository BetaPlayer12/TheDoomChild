using Holysoft.Collections;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
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
                GameSystem.sceneManager.LoadSceneAsync(m_introScene.sceneName);
                m_hasPlayedIntro = true;
            }
        }
    }
}