using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{

    public class StatToModelInjector : MonoBehaviour
    {
        [SerializeField]
        private PlayerStats m_stats;
        [SerializeField]
        private PlayerWeapon m_weapon;
        [SerializeField]
        private AttackResistance m_attackResistance;
        [SerializeField]
        private StatusEffectResistance m_statusResistance;

        [Title("Model Reference")]
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private StatusInflictor m_statusInflictor;
        [SerializeField]
        private AttackResistance m_modelAttackResistance;
        [SerializeField]
        private StatusEffectResistance m_modelStatusResistance;

        private void OnStatsChange(object sender, StatValueEventArgs eventArgs)
        {
            switch (eventArgs.stat)
            {
                case PlayerStat.Health:
                    m_health.SetMaxValue(eventArgs.value);
                    break;
                case PlayerStat.Magic:
                    m_magic.SetMaxValue(eventArgs.value);
                    break;
                case PlayerStat.Attack:
                case PlayerStat.MagicAttack:
                    m_attacker.SetDamage(CalculateDamage());
                    break;
                case PlayerStat._COUNT:
                    m_health.SetMaxValue(m_stats.GetStat(PlayerStat.Health));
                    m_magic.SetMaxValue(m_stats.GetStat(PlayerStat.Magic));
                    m_attacker.SetDamage(CalculateDamage());
                    break;
            }
        }

        private void InitializeValues()
        {
            m_health.SetMaxValue(m_stats.GetStat(PlayerStat.Health));
            m_health.ResetValueToMax();
            m_magic.SetMaxValue(m_stats.GetStat(PlayerStat.Magic));
            m_magic.ResetValueToMax();
            m_attacker.SetDamage(CalculateDamage());
            m_statusInflictor.SetInflictionList(m_weapon.statusInflictions);
            UpdateAttackResistance();
            UpdateStatusResistance();
        }

        private void UpdateAttackResistance()
        {
            var size = (int)AttackType._COUNT;
            for (int i = 0; i < size; i++)
            {
                var attackType = (AttackType)i;
                var resistance = m_attackResistance.GetResistance(attackType);
                m_modelAttackResistance.SetResistance(attackType, resistance);
            }
        }

        private void UpdateStatusResistance()
        {
            var size = (int)StatusEffectType._COUNT;
            for (int i = 0; i < size; i++)
            {
                var statusEffect = (StatusEffectType)i;
                var resistance = m_statusResistance.GetResistance(statusEffect);
                m_modelStatusResistance.SetResistance(statusEffect, resistance);
            }
        }

        private AttackDamage[] CalculateDamage()
        {
            var damageList = m_weapon.damage;
            var attack = m_stats.GetStat(PlayerStat.Attack);
            var magicAttack = m_stats.GetStat(PlayerStat.MagicAttack);
            for (int i = 0; i < damageList.Length; i++)
            {
                var damage = damageList[i];
                if (AttackDamage.IsMagicAttack(damageList[i].type))
                {
                    damage.value += magicAttack;
                }
                else
                {
                    damage.value += attack;
                }
                damageList[i] = damage;
            }
            return damageList;
        }

        private void OnDamageChange(object sender, EventActionArgs eventArgs)
        {
            m_attacker.SetDamage(CalculateDamage());
        }

        private void OnStatusInflictionUpdate(object sender, EventActionArgs eventArgs)
        {
            m_statusInflictor.SetInflictionList(m_weapon.statusInflictions);
        }

        private void OnAttackResistanceChange(object sender, AttackResistance.ResistanceEventArgs eventArgs)
        {
            if (eventArgs.type == AttackType._COUNT)
            {
                UpdateAttackResistance();
            }
            else
            {
                m_modelAttackResistance.SetResistance(eventArgs.type, eventArgs.resistanceValue);
            }
        }

        private void OnStatusResistanceChange(object sender, StatusEffectResistance.ResistanceEventArgs eventArgs)
        {
            if (eventArgs.type == StatusEffectType._COUNT)
            {
                UpdateStatusResistance();
            }
            else
            {
                m_modelStatusResistance.SetResistance(eventArgs.type, eventArgs.value);
            }
        }

        private void Start()
        {
            InitializeValues();
            m_stats.StatsChanged += OnStatsChange;
            m_weapon.DamageChange += OnDamageChange;
            m_weapon.StatusInflictionUpdate += OnStatusInflictionUpdate;
            m_attackResistance.ResistanceChange += OnAttackResistanceChange;
            m_statusResistance.ResistanceChange += OnStatusResistanceChange;
        }
    }
}