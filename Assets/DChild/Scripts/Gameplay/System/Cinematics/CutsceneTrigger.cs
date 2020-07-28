﻿/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using System;
using DChild.Gameplay.Characters.Players;
using DChild.Serialization;
using Holysoft.Event;
using PlayerNew;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Playables;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    [RequireComponent(typeof(Collider2D))]
    [DisallowMultipleComponent]
    public class CutsceneTrigger : MonoBehaviour, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isTriggered;

            public SaveData(bool isTriggered)
            {
                m_isTriggered = isTriggered;
            }

            public bool isTriggered => m_isTriggered;
        }

        [SerializeField]
        private PlayableDirector m_cutscene;

        private Collider2D m_collider;
        private PlayerControlledObject m_controlledObject;
        private Animator m_animator;
        private StateManager m_collisionState;
        private Scene m_originalScene;

        private bool m_isTriggered;

        public void Load(ISaveData data)
        {
            m_isTriggered = ((SaveData)data).isTriggered;
            if (m_collider != null)
            {
                m_collider.enabled = false;
            }
        }

        public ISaveData Save()
        {
            return new SaveData(m_isTriggered);
        }

        private void OnCutsceneDone(PlayableDirector obj)
        {
            m_controlledObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(m_controlledObject.gameObject, m_originalScene);
            m_animator.enabled = true;
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }

        private void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            if (m_isTriggered)
            {
                m_collider.enabled = false;
            }
            else
            {
                m_cutscene.stopped += OnCutsceneDone;

                var animation = GameplaySystem.playerManager.player.character.GetComponentInChildren<SkeletonAnimation>();
                var timelineAsset = m_cutscene.playableAsset as TimelineAsset;

                foreach (PlayableBinding binding in timelineAsset.GetOutputTrack(2).outputs)
                {
                    if (binding.sourceObject is SpineAnimationStateTrack)
                    {
                        var track = (TrackAsset)binding.sourceObject;
                        m_cutscene.SetGenericBinding(binding.sourceObject, animation);
                    }
                }
            }
            enabled = false;
        }

        private void LateUpdate()
        {
            if (m_collisionState.isGrounded)
            {
                m_animator.enabled = false;
                m_cutscene.Play();
                enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false && collision.CompareTag("DamageCollider") == false)
            {
                if (collision.TryGetComponentInParent(out PlayerControlledObject controlledObject))
                {
                    m_controlledObject = controlledObject;
                    m_originalScene = m_controlledObject.gameObject.scene;
                    m_controlledObject.transform.parent = m_cutscene.transform;
                    m_collisionState = m_controlledObject.GetComponentInChildren<StateManager>();
                    m_animator = m_controlledObject.GetComponentInChildren<Animator>();
                    GameplaySystem.playerManager.OverrideCharacterControls();

                    if (m_collisionState.isGrounded)
                    {
                        m_animator.enabled = false;
                        m_cutscene.Play();
                    }
                    else
                    {
                        enabled = true;
                    }
                    m_isTriggered = true;
                    m_collider.enabled = false;
                }
            }
        }
    }
}