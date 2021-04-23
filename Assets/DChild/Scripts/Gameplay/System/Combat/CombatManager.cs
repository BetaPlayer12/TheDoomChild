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
        Cache<AttackInfo> ResolveConflict(AttackerCombatInfo attacker, TargetInfo targetInfo);
        void Inflict(StatusEffectReciever reciever, StatusEffectType statusEffect);
        void Inflict(StatusEffectReciever reciever, params StatusEffectChance[] statusEffectChance);
        List<Hitbox> GetValidTargets(Vector2 source, Invulnerability ignoresLevel, List<Hitbox> hitboxes);
        List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer, Invulnerability ignoresLevel);
        void Damage(IDamageable damageable, AttackDamage damage);
        void Heal(IHealable healable, int health);
       
    }

    public class CombatManager : SerializedMonoBehaviour, ICombatManager, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private StatusInflictionHandle m_statusInflictionHandle;
        [SerializeField, HideLabel]
        private CriticalDamageHandle m_criticalDamageHandle;
        [SerializeField, HideLabel, Title("UI")]
        private CombatUIHandler m_uiHandler;
       
        private AOETargetHandler m_aOETargetHandler;
        private PlayerCombatHandler m_playerCombatHandler;
        private ResistanceHandler m_resistanceHandler;

        private List<AttackType> m_damageList;
        private IDamageable m_cacheTarget;

        public Cache<AttackInfo> ResolveConflict(AttackerCombatInfo attacker, TargetInfo targetInfo)
        {
            //AttackInfo result = new AttackInfo(attacker.damage);
            Cache<AttackInfo> result = Cache<AttackInfo>.Claim();
            result.Value.Initialize(attacker.damage);
            m_criticalDamageHandle.Execute(result, attacker.critChance, attacker.critDamageModifier);

            m_cacheTarget = targetInfo.instance;
            if (m_cacheTarget.attackResistance != null)
            {
                m_resistanceHandler.CalculatateResistanceReduction(m_cacheTarget.attackResistance, result);
            }

            ApplyResults(result, targetInfo.isCharacter);

            if (m_cacheTarget.isAlive)
            {
                if (targetInfo.isCharacter)
                {
                    if (targetInfo.isPlayer)
                    {
                        m_playerCombatHandler.ResolveDamageRecieved(targetInfo.owner);
                    }

                    if (targetInfo.flinchHandler != null)
                    {
                        FlinchTarget(targetInfo.flinchHandler, targetInfo.facing, m_cacheTarget.position, attacker.position, m_damageList);
                    }
                }
            }
            return result;
        }

        public void Inflict(StatusEffectReciever reciever, StatusEffectType statusEffect)
        {
            m_statusInflictionHandle.Inflict(reciever, statusEffect);
        }

        public void Inflict(StatusEffectReciever reciever, params StatusEffectChance[] statusEffectChance)
        {
            m_statusInflictionHandle.Inflict(reciever, statusEffectChance);
        }

        public void Damage(IDamageable damageable, AttackDamage attackDamage)
        {
            using (Cache<AttackInfo> cacheResult = Cache<AttackInfo>.Claim())
            {
                cacheResult.Value.Initialize(attackDamage);
                if (damageable.attackResistance != null)
                {
                    m_resistanceHandler.CalculatateResistanceReduction(damageable.attackResistance, cacheResult);
                }

                var damageInfo = cacheResult.Value.damageList[0];
                if (damageInfo.isHeal)
                {

                }
                else
                {
                    var damage = damageInfo.damage;
                    damageable.TakeDamage(damage.value, damage.type);
                    if (GameSystem.settings?.gameplay.showDamageValues ?? true)
                    {
                        m_uiHandler.ShowDamageValues(damageable.position, damage, false);
                    }
                }
                cacheResult.Release();
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

        public List<Hitbox> GetValidTargets(Vector2 source, Invulnerability ignoresLevel, List<Hitbox> hitboxes) => m_aOETargetHandler.ValidateTargets(source, ignoresLevel, hitboxes);
        public List<Hitbox> GetValidTargetsOfCircleAOE(Vector2 source, float radius, int layer, Invulnerability ignoresLevel) => m_aOETargetHandler.GetValidTargetsOfCircleAOE(source, radius, layer, ignoresLevel);

        public void Initialize()
        {
            m_uiHandler.Initialize(gameObject.scene);
            m_playerCombatHandler = GetComponentInChildren<PlayerCombatHandler>();
            m_resistanceHandler = new ResistanceHandler();
            m_aOETargetHandler = new AOETargetHandler();
        }

        private AttackInfo ApplyResults(AttackInfo result, bool isCharacter)
        {
            m_damageList.Clear();
            if (isCharacter && (GameSystem.settings?.gameplay.showDamageValues ?? true))
            {
                for (int i = 0; i < result.damageList.Count; i++)
                {
                    var damageInfo = result.damageList[i];
                    if (damageInfo.isHeal == false)
                    {
                        var damage = damageInfo.damage;
                        m_damageList.Add(damage.type);
                        m_cacheTarget.TakeDamage(damage.value, damage.type);
                        m_uiHandler.ShowDamageValues(m_cacheTarget.position, damage, false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < result.damageList.Count; i++)
                {
                    var damageInfo = result.damageList[i];
                    if (damageInfo.isHeal == false)
                    {
                        var damage = damageInfo.damage;
                        m_damageList.Add(damage.type);
                        m_cacheTarget.TakeDamage(damage.value, damage.type);
                    }
                }
            }

            return result;
        }

        private void FlinchTarget(IFlinch target, HorizontalDirection targetFacing, Vector2 targetPosition, Vector2 attackerPosition, IReadOnlyCollection<AttackType> attackType)
        {
            var damageSource = GameplayUtility.GetRelativeDirection(targetFacing, targetPosition, attackerPosition);
            var directionToSource = (attackerPosition - targetPosition).normalized;
            target.Flinch(directionToSource, damageSource, attackType);
        }

        private void Awake()
        {
            m_damageList = new List<AttackType>();
        }

        private void LateUpdate()
        {
            if (GameSystem.settings?.gameplay.showDamageValues ?? true)
            {
                m_uiHandler.Update();
            }
        }
    }
}