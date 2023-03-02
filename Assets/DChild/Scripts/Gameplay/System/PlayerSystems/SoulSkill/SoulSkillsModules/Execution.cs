using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
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
            private float m_cooldownTimer;
            private bool m_cooldown = false;

            public Handle(IPlayer m_reference, int hpThreshold, bool isPercentage, float m_cooldownTimer) : base(m_reference)
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
                if (m_cooldown == false)
                {
                    var targetHealth = eventArgs.target.instance.health;
                    Collider2D HitCollider = eventArgs.target.hitCollider;
                    bool instantlyKillTarget = false;
                    if (HitCollider.GetComponentInParent<Boss>() != null)
                        return;
                    if (m_isPercentage)
                    {
                        instantlyKillTarget = (targetHealth.currentValue / (float)targetHealth.maxValue) <= (m_hpThreshold / 100f);
                    }
                    else
                    {
                        instantlyKillTarget = targetHealth.currentValue <= m_hpThreshold;
                    }

                    if (instantlyKillTarget)
                    {
                        GameplaySystem.combatManager.Damage(eventArgs.target.instance, new Damage(DamageType.True, 9999999));
                        var monoBehaviour = (MonoBehaviour)sender;
                        if (monoBehaviour != null)
                        {
                            m_cooldown = true;
                            monoBehaviour.StopCoroutine("CooldownRoutine()");
                            monoBehaviour.StartCoroutine(CooldownRoutine());
                        }
                    }
                }
                

            }
            private IEnumerator CooldownRoutine()
            {

                yield return new WaitForSeconds(m_cooldownTimer);
                m_cooldown = false;
                yield return null;
            }
        }

        [SerializeField, MinValue(0)]
        private int m_hpThreshold;
        [SerializeField]
        private bool m_isPercentage;
        [SerializeField]
        private float m_cooldownTimer;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_hpThreshold, m_isPercentage, m_cooldownTimer);


        




    }
}