using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        [SerializeField]
        private PrimarySkill m_toUnlock;
        [SerializeField]
        private Transform m_prompt;
        [SerializeField]
        private ParticleSystem m_fx;
        [SerializeField, MinValue(0)]
        private float m_callNotificationDelay;
        [SerializeField]
        private bool m_isUsed;

        public bool showPrompt => m_isUsed == false;

        public string promptMessage => "Use";

        public Vector3 promptPosition => m_prompt.position;

        public ISaveData Save() => new SaveData(m_isUsed);

        public void Load(ISaveData data)
        {
            m_isUsed = ((SaveData)data).isUsed;
        }

        public void Interact(Character character)
        {
            if (m_isUsed == false)
            {
                GameplaySystem.playerManager.OverrideCharacterControls();
                switch (m_toUnlock)
                {
                    case PrimarySkill.BlackBloodImmunity:
                        character.GetComponentInChildren<BlackBloodImmunity>().isActive = true;
                        break;
                }
                m_fx.Play(true);
                StartCoroutine(DelayedNotifySkill());
                m_isUsed = true;
            }
        }

        private IEnumerator DelayedNotifySkill()
        {
            yield return new WaitForSeconds(m_callNotificationDelay);
            NotifySkill(m_toUnlock);
        }

        private void NotifySkill(PrimarySkill skill)
        {
            GameEventMessage.SendEvent("Primary Skill Acquired");
        }
    }
}