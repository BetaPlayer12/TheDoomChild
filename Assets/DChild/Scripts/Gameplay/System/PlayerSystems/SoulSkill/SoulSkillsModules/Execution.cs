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
            private GameObject m_instanceReference;
            private GameObject m_instance;


            public Handle(IPlayer m_reference, int hpThreshold, bool isPercentage, float m_cooldownTimer, GameObject m_instance) : base(m_reference)
            {
                m_hpThreshold = hpThreshold;
                m_isPercentage = isPercentage;
                this.m_instanceReference = m_instance;
            }

            public override void Dispose()
            {
                m_player.attackModule.TargetDamaged -= OnAttack;
                Object.Destroy(m_instance);
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
                    bool activateeffect = false;
                    Damage temp = m_player.weapon.damage;
                    float damage = temp.value;
                    if (HitCollider.GetComponentInParent<Boss>() != null)
                        return;
                    if (HitCollider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                        return;
                    if (m_isPercentage)
                    {
                        instantlyKillTarget = (targetHealth.currentValue / (float)targetHealth.maxValue) <= (m_hpThreshold / 100f);
                        activateeffect = (targetHealth.currentValue / (float)targetHealth.maxValue) <= (m_hpThreshold / 100f)+damage;
                    }
                    else
                    {
                        instantlyKillTarget = targetHealth.currentValue <= m_hpThreshold;
                        activateeffect = targetHealth.currentValue <= m_hpThreshold+damage;
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
                    if (activateeffect)
                    {
                        m_instance = Object.Instantiate(m_instanceReference);
                        
                        m_instance.transform.SetParent(eventArgs.target.instance.transform);
                        m_instance.transform.localPosition = new Vector3(0.0f, 12.0f, 0.0f);
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
        [SerializeField]
        private GameObject m_effects;

        protected override BaseHandle CreateHandle(IPlayer player) => new Handle(player, m_hpThreshold, m_isPercentage, m_cooldownTimer, m_effects);


        




    }
}