using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class Execution : HandledSoulSkillModule
    {
        private class Handle : BaseHandle
        {
            private int m_hpThreshold;
            private bool m_isPercentage;

            public Handle(IPlayer m_reference, int hpThreshold, bool isPercentage) : base(m_reference)
            {
                m_hpThreshold = hpThreshold;
                m_isPercentage = isPercentage;
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
                var targetHealth = eventArgs.target.instance.health;
                Collider2D HitCollider = eventArgs.target.hitCollider;
                bool instantlyKillTarget = false;
                if (HitCollider.GetComponentInParent<Boss>() != null)
                    return;
                if (m_isPercentage)
                {
                    instantlyKillTarget = (targetHealth.currentValue / (float) targetHealth.maxValue) <= (m_hpThreshold / 100f);
                }
                else
                {
                    instantlyKillTarget = targetHealth.currentValue <= m_hpThreshold;
                }

                if (instantlyKillTarget)
                {
                    GameplaySystem.combatManager.Damage(eventArgs.target.instance, new Damage(DamageType.True, 9999999));
                }
            }
        }

        [SerializeField, MinValue(0)]
        private int m_hpThreshold;
        [SerializeField]
        private bool m_isPercentage;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_hpThreshold, m_isPercentage);
    }
}