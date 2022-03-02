using DChild.Gameplay.Cinematics;
using DChild.Serialization;
using Doozy.Engine;
using PixelCrushers.DialogueSystem;
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
        private Transform m_playerStartPosition;
        [SerializeField]
        private AnimationReferenceAsset m_playerStandAnimation;
        [SerializeField]
        private DialogueSystemTrigger m_afterWakeupDialogue;
        [SerializeField]
        private GameObject m_storePickupSequence;

        private bool m_isDone;

        public ISaveData Save()
        {
            return new SaveData(m_isDone);
        }

        public void Load(ISaveData data)
        {
            m_isDone = ((SaveData)data).isDone;
            if (m_isDone == false)
            {
                m_introCutscene.Play();
            }
        }

        public void Initialize()
        {
            m_storePickupSequence.SetActive(false);
            m_introCutscene.Play();
        }

        public void TransferPlayerToStartPosition()
        {
            var player = GameplaySystem.playerManager.player.character;
            player.transform.position = m_playerStartPosition.position;
        }

        public void PromptPlayerToStand()
        {
            StopAllCoroutines();
            StartCoroutine(PromptPlayerToStandRoutine());
        }

        public void SetStorePickupSequence(bool startSequence)
        {
            m_storePickupSequence.SetActive(startSequence);
        }

        public void EndEvent()
        {
            m_isDone = true;
        }

        private IEnumerator PromptPlayerToStandRoutine()
        {
            GameplaySystem.playerManager.OverrideCharacterControls();
            var skeleton = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();

            GameEventMessage.SendEvent("Prompt_Wakeup_Start");
            bool hasPressedPrompt = false;
            while (hasPressedPrompt == false)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    hasPressedPrompt = true;
                }
                yield return null;
            }
            GameEventMessage.SendEvent("Prompt_Wakeup_Done");

            var standAnimation = skeleton.state.SetAnimation(0, m_playerStandAnimation, false);
            while (standAnimation.IsComplete == false)
            {
                yield return null;
            }

            m_afterWakeupDialogue.OnUse();

            SetStorePickupSequence(true);
            GameplaySystem.playerManager.StopCharacterControlOverride();
            yield return null;
        }
    }
}
