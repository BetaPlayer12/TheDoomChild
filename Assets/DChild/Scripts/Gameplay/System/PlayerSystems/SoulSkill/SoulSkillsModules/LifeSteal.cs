using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class LifeSteal : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private IPlayer m_reference;
            private float m_lifeSteal;

            public Handle(IPlayer m_reference, int m_lifeSteal)
            {
                this.m_reference = m_reference;
                this.m_lifeSteal = m_lifeSteal / 100f;
            }

            public override void Dispose()
            {
                m_reference.attackModule.TargetDamaged -= OnAttack;
            }

            public override void Initialize()
            {
                m_reference.attackModule.TargetDamaged += OnAttack;
            }

            private void OnAttack(object sender, CombatConclusionEventArgs eventArgs)
            {
                var heal = eventArgs.result.totalDamageDealt * m_lifeSteal;
                GameplaySystem.combatManager.Heal(m_reference.healableModule, Mathf.CeilToInt(heal));
            }
        }

        [SerializeField, MinValue(1), SuffixLabel("%", overlay: true)]
        private int m_healBasedOnDamage;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_healBasedOnDamage);
    }
}