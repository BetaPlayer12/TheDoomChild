using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class DamageReductionHandle : IStatusEffectModule
    {
        [System.Serializable]
        private struct Info
        {
               
            [SerializeField]
            private float m_value;
            public float value => m_value;
        }

        [SerializeField]
        private Info[] m_infos;

        public IStatusEffectModule GetInstance() => this;


        public void Start(Character character)
        {
            var playerControlledObject = character.GetComponent<PlayerControlledObject>();
            if (playerControlledObject != null)
            {
                var stats = playerControlledObject.owner.stats;
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    playerControlledObject.owner.modifiers.Add(PlayerModifier.AttackDamage,info.value);
                }
            }
            else
            {
                var characterObject = character.GetComponent<Attacker>();
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    characterObject.SetDamageModifier(characterObject.modifier + info.value);
                }
                
            }
        }

        public void Stop(Character character)
        {
            var playerControlledObject = character.GetComponent<PlayerControlledObject>();
            if (playerControlledObject != null)
            {
                var stats = playerControlledObject.owner.stats;
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    playerControlledObject.owner.modifiers.Add(PlayerModifier.AttackDamage, - info.value);
                }
            }
            else
            {
                var characterObject = character.GetComponent<Attacker>();
                var baseModifier = characterObject.modifier;
                for (int i = 0; i < m_infos.Length; i++)
                {
                    var info = m_infos[i];
                    characterObject.SetDamageModifier(baseModifier - info.value);
                }

            }
        }
    }
}

