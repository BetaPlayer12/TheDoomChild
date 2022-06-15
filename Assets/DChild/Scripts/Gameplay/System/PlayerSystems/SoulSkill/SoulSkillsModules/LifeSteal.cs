using System;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [System.Serializable]
    public class LifeSteal : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private float m_lifeSteal;

            public Handle(IPlayer m_reference, int m_lifeSteal) : base(m_reference)
            {
                this.m_lifeSteal = m_lifeSteal / 100f;
            }

            public override void Dispose()
            {
                m_player.attackModule.TargetDamaged -= OnAttack;
            }

            public override void Initialize()
            {
                m_player.attackModule.TargetDamaged += OnAttack;
            }

            private void OnAttack(object sender, CombatConclusionEventArgs eventArgs)
            {
                var heal = eventArgs.result.damageDealt * m_lifeSteal;
                GameplaySystem.combatManager.Heal(m_player.healableModule, Mathf.CeilToInt(heal));
            }
        }

        [SerializeField, MinValue(1), SuffixLabel("%", overlay: true)]
        private int m_healBasedOnDamage;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_healBasedOnDamage);
    }
}