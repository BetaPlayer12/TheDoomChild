using DChild.Gameplay.Cinematics;
using DChild.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Narrative
{
    public class NewGameIntroEvent : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isDone;

            public SaveData(bool isDone)
            {
                m_isDone = isDone;
            }

            public bool isDone => m_isDone;

            public ISaveData ProduceCopy() => new SaveData(m_isDone);

        }

        [SerializeField]
        private PlayableDirector m_introCutscene;
        [SerializeField]
        private CutsceneTrigger m_introCutsceneTrigger;
        [SerializeField]
        private GameObject m_tutorialTriggerGroup;

        private bool m_isDone;

#if UNITY_EDITOR
        [SerializeField]
        private bool m_skipThis;
#endif

        public void EndEvent()
        {
            m_isDone = true;
            m_tutorialTriggerGroup.SetActive(false);
        }

        public void Load(ISaveData data)
        {
            m_isDone = ((SaveData)data).isDone;
#if UNITY_EDITOR
            if (m_skipThis)
            {
                m_isDone = true;
            }
#endif
        }
        

        public ISaveData Save()
        {
            return new SaveData(m_isDone);
        }
        public void Initialize()
        {
            m_isDone = false;
        }

        public void StartEvent()
       {
#if UNITY_EDITOR
            if (m_skipThis)
            {
                return;
            }
#endif
            m_isDone = true;
            m_introCutsceneTrigger.ForcePlayCutscene();
        }

        private void OnCutsceneEnd(PlayableDirector obj)
        {
            m_tutorialTriggerGroup.SetActive(true);
        }

        private void Awake()
        {
            m_introCutscene.stopped += OnCutsceneEnd;
            m_tutorialTriggerGroup.SetActive(false);
        }
    }
}
