using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using Doozy.Engine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay
{
    public class PlayerSkillUnlocker : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        private struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isUsed;

            public SaveData(bool isUsed) : this()
            {
                m_isUsed = isUsed;
            }
            public bool isUsed => m_isUsed;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isUsed);
        }

        [SerializeField]
        private PrimarySkill m_toUnlock;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private PlayableDirector m_cinematic;
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField, OnValueChanged("OnIsUsedChanged")]
        private bool m_isUsed;

        public bool showPrompt => true;

        public string promptMessage => "Use";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public ISaveData Save() => new SaveData(m_isUsed);

        public void Load(ISaveData data)
        {
            m_isUsed = ((SaveData)data).isUsed;
            m_collider.enabled = !m_isUsed;
        }

        public void Interact(Character character)
        {
            if (m_isUsed == false)
            {
                if (character)
                {
                    character.GetComponent<PlayerControlledObject>().owner.skills.SetSkillStatus(m_toUnlock, true);
                }

                if (m_cinematic == null)
                {
                    GameplaySystem.gamplayUIHandle.PromptPrimarySkillNotification();
                }
                else
                {
                    m_cinematic.Play();
                }

                m_isUsed = true;
                m_collider.enabled = false;
            }
        }

        private void OnCutsceneDone(PlayableDirector obj)
        {
            NotifySkill(m_toUnlock);
        }

        private void NotifySkill(PrimarySkill skill)
        {
            GameplaySystem.gamplayUIHandle.PromptPrimarySkillNotification();
        }

        private void Awake()
        {
            m_cinematic.stopped += OnCutsceneDone;
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }

#if UNITY_EDITOR
        private void OnIsUsedChanged()
        {
            m_collider.enabled = !m_isUsed;
        }

        [Button]
        private void Interact()
        {
            Interact(null);
        }
#endif
    }
}