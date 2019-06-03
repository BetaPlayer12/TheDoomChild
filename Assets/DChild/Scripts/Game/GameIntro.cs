using Holysoft;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace DChild
{
    public class GameIntro : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer m_companyLogo;
        [SerializeField]
        private UICanvas m_trademark;
        [SerializeField]
        private DelayedEvent m_event;
        [SerializeField]
        private UIAnimation m_tradeMarkAnimation;
        [SerializeField]
        private SceneInfo m_mainMenu;

        private void OnTrademarkEnd(object sender, EventActionArgs eventArgs)
        {
            SceneManager.LoadSceneAsync(m_mainMenu.sceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private void OnEndReached(VideoPlayer source)
        {
            source.enabled = false;
            m_trademark.Show();
            m_event.StartEvent();
        }

        private void Awake()
        {
            m_tradeMarkAnimation.AnimationEnd += OnTrademarkEnd;
        }

        private void Start() => m_companyLogo.loopPointReached += OnEndReached;
    }

}