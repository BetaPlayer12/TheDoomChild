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
                Addressables.LoadSceneAsync(m_introScene.sceneName, LoadSceneMode.Additive).Completed += OnIntroSceneLoaded;
                m_hasPlayedIntro = true;
            }
        }

        private void OnIntroSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            GameIntro.currentIntroScene = obj.Result;
        }
    }
}