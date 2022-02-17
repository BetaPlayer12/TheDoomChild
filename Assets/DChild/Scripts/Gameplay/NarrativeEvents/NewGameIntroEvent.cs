using DChild.Gameplay.Cinematics;
using DChild.Serialization;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        private AnimationReferenceAsset m_playerStandAnimation;
        [SerializeField]
        private GameObject m_storePickupSequence;

        private bool m_isDone;

        public ISaveData Save()
        {
            return new SaveData(m_isDone);
        }

        public void Load(ISaveData data)
        {

        }

        public void Initialize()
        {
            m_storePickupSequence.SetActive(false);
        }

        public void PromptPlayerToStand()
        {
            Debug.Log("Player Prompt Show");
            StopAllCoroutines();
            StartCoroutine(PromptPlayerToStandRoutine());
        }

        public void SetStorePickupSequence(bool startSequence)
        {
            m_storePickupSequence.SetActive(!startSequence);
        }

        public void EndEvent()
        {
            m_isDone = true;
        }

        private IEnumerator PromptPlayerToStandRoutine()
        {
            var skeleton = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();

            bool hasPressedPrompt = false;
            while (hasPressedPrompt == false)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    hasPressedPrompt = true;
                }
                yield return null;
            }

            var standAnimation = skeleton.state.SetAnimation(0, m_playerStandAnimation, false);
            while(standAnimation.IsComplete ==false)
            {
                yield return null;
            }

            GameplaySystem.playerManager.StopCharacterControlOverride();
            yield return null;
        }
    }
}
