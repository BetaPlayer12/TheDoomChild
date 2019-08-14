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
            private IPlayer m_reference;
            private int m_healValue;

            public Handle(IPlayer m_reference, int m_healValue)
            {
                this.m_reference = m_reference;
                this.m_healValue = m_healValue;
            }

            public override void Dispose()
            {
                m_reference.attackModule.TargetDamaged -= OnTargetDamaged;
            }

            public override void Initialize()
            {
                m_reference.attackModule.TargetDamaged += OnTargetDamaged;
            }

            private void OnTargetDamaged(object sender, CombatConclusionEventArgs eventArgs)
            {
                if (m_reference.health.isFull == false)
                {
                    if (eventArgs.target.isCharacter && eventArgs.result.totalDamageDealt > 0)
                    {
                        GameplaySystem.combatManager.Heal(m_reference.healableModule, m_healValue);
                    }
                }
            }
        }

        [SerializeField, MinValue(1)]
        private int m_healValue;

        protected override BaseHandle CreateHandle(IPlayer player)
        {
            return new Handle(player, m_healValue);
        }
    }
}