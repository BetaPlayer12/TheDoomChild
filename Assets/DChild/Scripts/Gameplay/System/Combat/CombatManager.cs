using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Combat.UI;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface ICombatManager
    {
        AttackSummaryInfo ResolveConflict(AttackerCombatInfo attacker, TargetInfo targetInfo);
        void Inflict(StatusEffectReciever reciever, StatusEffectType statusEffect);
        void Inflict(StatusEffectReciever reciever, params StatusEffectChance[] statusEffectChance);
        List<Hitbox> GetValidTargets(Vector2 source, Invulnerability ignoresLevel, List<Hitbox> hitboxes);
        List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer, Invulnerability ignoresLevel);
        void Damage(IDamageable damageable, Damage damage);
        void Heal(IHealable healable, int health);

    }

    public class CombatManager : SerializedMonoBehaviour, ICombatManager, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private StatusInflictionHandle m_statusInflictionHandle;
        [SerializeField, HideLabel]
        private DamageCalculator m_damageCalculator;
        [SerializeField, HideLabel, Title("UI")]
        private CombatUIHandler m_uiHandler;

        private AOETargetHandler m_aOETargetHandler;
        private PlayerCombatHandler m_playerCombatHandler;

        private IDamageable m_cacheTarget;

        private bool showCombatValues => GameSystem.settings?.gameplay.showDamageValues ?? true;

        public AttackSummaryInfo ResolveConflict(AttackerCombatInfo attacker, TargetInfo targetInfo)
        {
            AttackSummaryInfo summary = m_damageCalculator.CalculateDamage(attacker.attackInfo, targetInfo.instance.attackResistance, targetInfo.canBlockDamage, DamageCalculator.Operations.All);
            m_cacheTarget = targetInfo.instance;
            ApplyAttackDamage(summary, m_cacheTarget, targetInfo.isCharacter); //reference Struct

            if (m_cacheTarget.isAlive)
            {
                if (targetInfo.isCharacter)
                {
                    if (summary.wasBlocked)
                    {
                        targetInfo.instance.transform.GetComponentInChildren<IBlock>()?.BlockAttack(attacker.instance);
                    }
                    else
                    {
                        if (targetInfo.isPlayer)
                        {
                            m_playerCombatHandler.ResolveDamageRecieved(targetInfo.owner);
                        }

                        if (targetInfo.flinchHandler != null)
                        {
                            FlinchTarget(targetInfo.flinchHandler, targetInfo.facing, m_cacheTarget.position, attacker.position, summary);
                        }
                    }
                }
            }
            if (m_cacheTarget.transform.TryGetComponent(out IDamageReaction damageReaction))
            {
                damageReaction.ReactToBeingAttackedBy(attacker.instance, summary.wasBlocked);
            }

            if (attacker.isPlayer)
            {

            }

            return summary;
        }



        public void Inflict(StatusEffectReciever reciever, StatusEffectType statusEffect)
        {
            m_statusInflictionHandle.Inflict(reciever, statusEffect);
        }

        public void Inflict(StatusEffectReciever reciever, params StatusEffectChance[] statusEffectChance)
        {
            m_statusInflictionHandle.Inflict(reciever, statusEffectChance);
        }

        public void Damage(IDamageable damageable, Damage attackDamage)
        {
            var result = m_damageCalculator.CalculateDamage(new AttackInfo(attackDamage), null, false, DamageCalculator.Operations.DamageResistance);
            var damageInfo = result.damageInfo;
            var damage = damageInfo.damage;
            if (damageInfo.isHeal)
            {
                var healable = (IHealable)damageable;
                Heal(healable, damage.value);
            }
            else
            {
                damageable.TakeDamage(damage.value, damage.type);
                if (showCombatValues)
                {
                    m_uiHandler.ShowDamageValues(damageable.position, damage, false);
                }
            }
        }

        public void Heal(IHealable healable, int health)
        {
            healable.Heal(health);
            if (showCombatValues)
            {
                m_uiHandler.ShowHealValues(healable.position, health, false);
            }
        }

        public List<Hitbox> GetValidTargets(Vector2 source, Invulnerability ignoresLevel, List<Hitbox> hitboxes) => m_aOETargetHandler.ValidateTargets(source, ignoresLevel, hitboxes);
        public List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer, Invulnerability ignoresLevel) => m_aOETargetHandler.GetValidTargetsOfCircleAOE(source, radius, layer, ignoresLevel);

        public void Initialize()
        {
            m_uiHandler.Initialize(gameObject.scene);
            m_playerCombatHandler = GetComponentInChildren<PlayerCombatHandler>();
            m_aOETargetHandler = new AOETargetHandler();
        }

        private void ApplyAttackDamage(AttackSummaryInfo attackInfo, IDamageable target, bool isCharacter)
        {
            var showDamageValues = isCharacter && showCombatValues;
            var damageInfo = attackInfo.damageInfo;
            if (damageInfo.isHeal == false)
            {
                var damage = damageInfo.damage;
                target.TakeDamage(damage.value, damage.type);
                if (showCombatValues)
                {
                    m_uiHandler.ShowDamageValues(target.position, damage, attackInfo.isCrit);
                }
            }
        }

        private void FlinchTarget(IFlinch target, HorizontalDirection targetFacing, Vector2 targetPosition, Vector2 attackerPosition, AttackSummaryInfo attackInfo)
        {
            var damageSource = GameplayUtility.GetRelativeDirection(targetFacing, targetPosition, attackerPosition);
            var directionToSource = (attackerPosition - targetPosition).normalized;
            target.Flinch(directionToSource, damageSource, attackInfo);
        }

        private void LateUpdate()
        {
            if (showCombatValues)
            {
                m_uiHandler.Update();
            }
        }
    }
}