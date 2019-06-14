using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusInfliction;
using DChild.Gameplay.Systems;
using DChild.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface ICombatManager
    {
        DamageInfo ResolveConflict(AttackInfo attacker, TargetInfo targetInfo);
        void InflictStatusTo(IStatusReciever statusReciever, StatusEffectType type);
        void CureStatusOf(IStatusReciever statusReciever, StatusEffectType type);
        List<Hitbox> GetValidTargets(Vector2 source, List<Hitbox> hitboxes);
        List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer);
        void InflictStatusTo(IStatusReciever statusReciever, params StatusInflictionInfo[] statusInflictionInfos);
        void Damage(IDamageable damageable, AttackDamage damage);
        void Heal(IHealable healable, int health);
    }

    public class CombatManager : SerializedMonoBehaviour, ICombatManager, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField, HideLabel]
        private CriticalDamageHandler m_criticalDamageHandler;
        [NonSerialized, OdinSerialize, HideReferenceObjectPicker, HideLabel, Title("Status Infliction")]
        private StatusEffectManager m_statusEffectManager = new StatusEffectManager();
        [SerializeField, HideLabel, Title("UI")]
        private CombatUIHandler m_uiHandler;
        private AOETargetHandler m_aOETargetHandler;

        private PlayerCombatHandler m_playerCombatHandler;
        private ResistanceHandler m_resistanceHandler;

        public void InflictStatusTo(IStatusReciever statusReciever, StatusEffectType type) => m_statusEffectManager.InflictStatusTo(statusReciever, type);
        public void CureStatusOf(IStatusReciever statusReciever, StatusEffectType type) => m_statusEffectManager.CureStatusOf(statusReciever, type);
        public void InflictStatusTo(IStatusReciever statusReciever, params StatusInflictionInfo[] statusInflictionInfos) => m_statusEffectManager.InflictStatusTo(statusReciever, statusInflictionInfos);
        private ITarget m_cacheTarget;


        public DamageInfo ResolveConflict(AttackInfo attacker, TargetInfo targetInfo)
        {
            DamageInfo result = new DamageInfo(attacker.damage);
            m_criticalDamageHandler.Execute(ref result, attacker.critChance, attacker.critDamageModifier);
            if (targetInfo.damageReduction > 0)
            {
                result.damage -= Mathf.FloorToInt(result.damage * targetInfo.damageReduction);
            }

            m_cacheTarget = targetInfo.target;
            if (m_cacheTarget.attackResistance != null)
            {
                m_resistanceHandler.CalculatateResistanceReduction(m_cacheTarget.attackResistance, ref result);
            }
            if (DChildUtility.HasInterface<IPlayerCombat>(m_cacheTarget))
            {
                ResolveConflictToPlayer(attacker, (IPlayerCombat)m_cacheTarget, ref result);
            }

            if (result.isHeal)
            {
                m_cacheTarget.Heal(result.damage);
                if (GameSystem.settings?.gameplay.showDamageValues ?? true)
                {
                    m_uiHandler.ShowHealValues(m_cacheTarget.position, result.damage, result.isCrit);
                }
            }
            else
            {
                m_cacheTarget.TakeDamage(result.damage, result.damageType);
                if (GameSystem.settings?.gameplay.showDamageValues ?? true)
                {
                    m_uiHandler.ShowDamageValues(m_cacheTarget.position, new AttackDamage(result.damageType, result.damage), false);
                }
            }
            
            if(DChildUtility.HasInterface<IFlinch>(m_cacheTarget))
            {
                FlinchTarget((IFlinch)m_cacheTarget, m_cacheTarget.position, attacker.position, result.damageType);
            }

            return result;
        }

        public void Damage(IDamageable damageable, AttackDamage damage)
        {
            DamageInfo result = new DamageInfo(damage);
            if (damageable.attackResistance != null)
            {
                m_resistanceHandler.CalculatateResistanceReduction(damageable.attackResistance, ref result);
            }
            if (DChildUtility.HasInterface<IPlayerCombat>(damageable))
            {
                m_playerCombatHandler.FactorDefense((IPlayerCombat)damageable, ref result);
            }

            damageable.TakeDamage(result.damage, result.damageType);
            if (GameSystem.settings?.gameplay.showDamageValues ?? true)
            {
                m_uiHandler.ShowDamageValues(damageable.position, new AttackDamage(result.damageType, result.damage), false);
            }
        }

        public void Heal(IHealable healable, int health)
        {
            healable.Heal(health);
            if (GameSystem.settings?.gameplay.showDamageValues ?? true)
            {
                m_uiHandler.ShowHealValues(healable.position, health, false);
            }
        }

        public List<Hitbox> GetValidTargets(Vector2 source, List<Hitbox> hitboxes) => m_aOETargetHandler.ValidateTargets(source, hitboxes);
        public List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer) => m_aOETargetHandler.GetValidTargetsOfCircleAOE(source, radius, layer);

        public void Initialize()
        {
            m_uiHandler.Initialize(gameObject.scene);
            m_statusEffectManager.Initialize();
            m_playerCombatHandler = GetComponentInChildren<PlayerCombatHandler>();
            m_resistanceHandler = new ResistanceHandler();
            m_aOETargetHandler = new AOETargetHandler();
        }

        private void FlinchTarget(IFlinch target, Vector2 targetPosition, Vector2 attackerPosition, AttackType attackType)
        {
            if (target.isAlive)
            {
                var targetFacing = ((IFacing)target).currentFacingDirection;
                var damageSource = GameplayUtility.GetRelativeDirection(targetFacing, targetPosition, attackerPosition);
                target.Flinch(damageSource, attackType);
            }
        }

        private void ResolveConflictToPlayer(AttackInfo attacker, IPlayerCombat player, ref DamageInfo info)
        {
            //m_playerCombatHandler.FactorDefense(player, ref info);
            //m_playerCombatHandler.ResolveDamageRecieved(player, attacker.position);
            FlinchTarget((IFlinch)player, player.position, attacker.position, info.damageType);
        }

        private void LateUpdate()
        {
            m_statusEffectManager.Update();
            if (GameSystem.settings?.gameplay.showDamageValues ?? true)
            {
                m_uiHandler.Update();
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            m_statusEffectManager.OnValidate();
#endif
        }
    }
}