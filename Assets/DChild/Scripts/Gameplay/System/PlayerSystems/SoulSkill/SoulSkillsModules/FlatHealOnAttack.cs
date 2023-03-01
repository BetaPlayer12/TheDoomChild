using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class FlatHealOnAttack : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private int m_healValue;
            private GameObject m_instanceReference;
            private GameObject m_instance;

            public Handle(IPlayer m_reference, int healValue, GameObject m_instance) : base(m_reference)
            {
                m_healValue = healValue;
                this.m_instanceReference = m_instance;
            }

            public override void Dispose()
            {
                m_player.attackModule.TargetDamaged -= OnTargetDamaged;
                Object.Destroy(m_instance);
            }

            public override void Initialize()
            {
                m_player.attackModule.TargetDamaged += OnTargetDamaged;
            }

            private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
            {
                if (m_player.health.isFull == false)
                {
                    if (eventArgs.target.isCharacter && eventArgs.result.damageDealt > 0)
                    {
                        GameplaySystem.combatManager.Heal(m_player.healableModule, m_healValue);
                        m_instance = Object.Instantiate(m_instanceReference);
                        m_instance.transform.SetParent(m_player.character.transform);
                        m_instance.transform.localPosition =new Vector3(0.0f, 12.0f, 0.0f);
                    }
                }
               
            }
        }

        [SerializeField, MinValue(1)]
        private int m_healValue;
        [SerializeField]
        private GameObject m_effects;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_healValue, m_effects);
    }
}