﻿using Cinemachine;
using DChild.Gameplay.Cinematics;
using DChild.Serialization;
using DChild.Temp;
using Doozy.Runtime.UIManager.Containers;
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
        private CinemachineVirtualCamera m_cameraToDisable;
        [SerializeField]
        private UIContainer m_wakeUpPrompt;
        [SerializeField]
        private AnimationReferenceAsset m_playerStandAnimation;
        [SerializeField]
        private DialogueSystemTrigger m_afterWakeupDialogue;
        [SerializeField]
        private GameObject m_storePickupSequence;
        [SerializeField]
        private ExtraDatabases m_database;
        [SerializeField]
        private InputActionReference m_wakeUpInput;

        private bool m_isDone;
        bool hasPressedPrompt = false;

        private void OnInputPerformed(InputAction.CallbackContext context)
        {
            hasPressedPrompt = true;
        }

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
            m_database.OnUse();
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
            GameplaySystem.playerManager.player.GetComponentInChildren<PlayerInput>().actions.FindActionMap("Gameplay").Enable();
            var skeleton = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();
            m_wakeUpPrompt.Show();

            yield return WakeupPromptRoutine();

            var standAnimation = skeleton.state.SetAnimation(0, m_playerStandAnimation, false);
            while (standAnimation.IsComplete == false)
            {
                yield return null;
            }

            Debug.Log("Wake Up Animation Completed");
            GameplaySystem.playerManager.StopCharacterControlOverride();
            Debug.Log("Returned Player Controlls");
            m_afterWakeupDialogue.OnUse();
            Debug.Log("Wake Up Dialogue Initialized");
            SetStorePickupSequence(true);
            yield return null;
        }

        private IEnumerator WakeupPromptRoutine()
        {
            hasPressedPrompt = false;
            m_wakeUpInput.action.performed += OnInputPerformed;
            while (hasPressedPrompt == false)
            {
                yield return null;
            }
            m_wakeUpInput.action.performed -= OnInputPerformed;
            m_wakeUpPrompt.Hide();
            m_cameraToDisable.enabled = false;
        }
        public IEnumerator PlayerActionChange(Action<PlayerInput> callBack)
        {
            var actionMap = m_wakeUpInput.action;
            actionMap.performed += OnInputPerformed;

            yield return null;
        }
    } 

}
