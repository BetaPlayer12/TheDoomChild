/************************************
 * 
 * A Cutscene is Played when player
 * is inside the trigger
 * 
 ************************************/

using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
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

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isTriggered);
        }

        [SerializeField]
        private PlayableDirector m_cutscene;
        [SerializeField]
        private PlayableAsset m_cinematic;

        private Collider2D m_collider;
        private PlayerControlledObject m_controlledObject;
        private Animator m_animator;
        private CharacterState m_collisionState;
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

        public void ForcePlayCutscene()
        {
            StartCutscene(GameplaySystem.playerManager.player.character.GetComponent<PlayerControlledObject>());
        }

        private void OnCutsceneDone(PlayableDirector obj)
        {
            m_controlledObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(m_controlledObject.gameObject, m_originalScene);
            m_animator.enabled = true;
            GameplaySystem.playerManager.StopCharacterControlOverride();
        }

        private void StartCutscene(PlayerControlledObject controlledObject)
        {
            m_controlledObject = controlledObject;
            m_originalScene = m_controlledObject.gameObject.scene;
            m_controlledObject.transform.parent = m_cutscene.transform;
            m_collisionState = m_controlledObject.owner.state;
            var rigidBody = controlledObject.GetComponent<Rigidbody2D>();
            var velocity = rigidBody.velocity;
            velocity.x = 0;
            GameplaySystem.playerManager.OverrideCharacterControls();
            rigidBody.velocity = velocity;
            m_animator = m_controlledObject.GetComponentInChildren<Animator>();

            if (m_collisionState.isGrounded)
            {
                m_animator.enabled = false;
                if (m_cinematic != null)
                {
                    m_cutscene.playableAsset = m_cinematic;
                }
                m_cutscene.Play();
            }
            else
            {
                enabled = true;
            }
            m_isTriggered = true;
            m_collider.enabled = false;
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
                if (m_cinematic != null)
                {
                    m_cutscene.playableAsset = m_cinematic;
                }
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
                    StartCutscene(controlledObject);
                }
            }
        }

#if UNITY_EDITOR
        [Button]
        private void PlayCutscene()
        {
            m_cutscene.Play();
        }
#endif
    }
}