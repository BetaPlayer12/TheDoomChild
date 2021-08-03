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
            Cache<AttackInfo> result = Cache<AttackInfo>.Claim();
            result.Value.Initialize(attacker.damage);
            m_criticalDamageHandle.Execute(result, attacker.critChance, attacker.critDamageModifier);

            m_cacheTarget = targetInfo.instance;

            HandleDamageBlock(attacker, targetInfo, result);
            var wasDamageBlocked = result.Value.wasBlocked;
            if (wasDamageBlocked == false)
            {
                HandleDamageResistance(m_cacheTarget.attackResistance, result);
            }
            ApplyAttackResults(result, targetInfo.isCharacter);

            if (m_cacheTarget.isAlive)
            {
                if (targetInfo.isCharacter)
                {
                    if (wasDamageBlocked)
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
                            FlinchTarget(targetInfo.flinchHandler, targetInfo.facing, m_cacheTarget.position, attacker.position, m_damageList);
                        }
                    }
                }
            }

            if(m_cacheTarget.transform.TryGetComponent(out IDamageReaction damageReaction))
            {
                damageReaction.ReactToBeingAttackedBy(attacker.instance, result.Value.wasBlocked);
            }

            if (attacker.isPlayer)
            {

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
                HandleDamageResistance(damageable.attackResistance, cacheResult);

                var damageInfo = cacheResult.Value.damageList[0];
                var damage = damageInfo.damage;
                if (damageInfo.isHeal)
                {
                    var healable = (IHealable)damageable;
                    Heal(healable, damage.value);
                }
                else
                {
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

        private AttackInfo ApplyAttackResults(AttackInfo result, bool isCharacter)
        {
            m_damageList.Clear();
            var showDamageValues = isCharacter && (GameSystem.settings?.gameplay.showDamageValues ?? true);
            for (int i = 0; i < result.damageList.Count; i++)
            {
                var damageInfo = result.damageList[i];
                if (damageInfo.isHeal == false)
                {
                    var damage = damageInfo.damage;
                    m_damageList.Add(damage.type);
                    m_cacheTarget.TakeDamage(damage.value, damage.type);
                    if (showDamageValues)
                    {
                        m_uiHandler.ShowDamageValues(m_cacheTarget.position, damage, false);
                    }
                }
            }

            return result;
        }

        private void HandleDamageResistance(IAttackResistance resistance, AttackInfo info)
        {
            if (resistance != null)
            {
                m_resistanceHandler.CalculatateResistanceReduction(resistance, info);
            }
        }

        private void HandleDamageBlock(AttackerCombatInfo attackerInfo, TargetInfo targetInfo, AttackInfo attackInfo)
        {
            if (targetInfo.canBlockDamage && attackerInfo.ignoresBlock == false)
            {
                var list = attackInfo.damageList;
                for (int i = 0; i < list.Count; i++)
                {
                    var damageInfo = list[i];
                    var damage = damageInfo.damage;
                    damage.value = 0;
                    damageInfo.damage = damage;
                    list[i] = damageInfo;
                }
                attackInfo.wasBlocked = true;
            }
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