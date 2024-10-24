using DChild.Menu;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Gameplay.Systems
{
    public class BaseGameplayUIHandle : MonoBehaviour, IGameplaySystemModule, IGameplayInitializable
    {
        public static BaseGameplayUIHandle Instance { get; private set; }

        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_cinemaSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_gameOverSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_confirmationWindowSignal;


        [SerializeField]
        private ConfirmationHandler m_confirmationWindow;


        [SerializeField]
        private UIContainer m_skippableUI;
        [SerializeField]
        private UIContainer m_fadeUI;


        [SerializeField]
        private CinematicVideoHandle m_cinematicVideoHandle;
        [SerializeField]
        private UIView m_cinematicBars;

        public void ToggleCinematicMode(bool on)
        {
            m_cinemaSignal.Payload.booleanValue = on;
            m_cinemaSignal.SendSignal();
        }

        public void ToggleCinematicBars(bool value)
        {
            if (value)
            {
                m_cinematicBars.Show();
            }
            else
            {
                m_cinematicBars.Hide();
            }
        }

        public void ToggleFadeUI(bool willshow)
        {
            if (willshow)
            {
                m_fadeUI.Show();
            }
            else
            {
                m_fadeUI.Hide();
            }
        }

        public void ShowGameOverScreen()
        {
            m_gameOverSignal.SendSignal();
        }


        public void ShowSequenceSkip(bool willShow)
        {
            if (willShow)
            {
                m_skippableUI.Show();
            }
            else
            {
                m_skippableUI.Hide();
            }
        }

        public void Initialize()
        {
            m_cinematicVideoHandle.Initialize();
        }

        public void ShowCinematicVideo(VideoClip clip, Func<IEnumerator> behindTheSceneRoutine = null, Action OnVideoDone = null)
        {
            m_cinematicVideoHandle.ShowCinematicVideo(clip, behindTheSceneRoutine, OnVideoDone);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (Instance != null)
            {
                Instance = null;
            }
        }
    }
}