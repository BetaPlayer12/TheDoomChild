using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
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


        [Title("Model Reference")]
        [SerializeField]
        private Health m_health;
        [SerializeField]
        private Magic m_magic;
        [SerializeField]
        private Attacker m_attacker;
        [SerializeField]
        private AttackResistance m_modelAttackResistance;

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
            UpdateResistance();
        }

        private void UpdateResistance()
        {
            for (int i = 0; i < (int)AttackType._COUNT; i++)
            {
                var attackType = (AttackType)i;
                var resistance = m_attackResistance.GetResistance(attackType);
                m_modelAttackResistance.SetResistance(attackType, resistance);
            }
        }

        private AttackDamage[] CalculateDamage()
        {
           var damageList =  m_weapon.damage;
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

        private void OnResistanceChange(object sender, AttackResistance.ResistanceEventArgs eventArgs)
        {
            if (eventArgs.type == AttackType._COUNT)
            {
                UpdateResistance();
            }
            else
            {
                m_modelAttackResistance.SetResistance(eventArgs.type, eventArgs.resistanceValue);
            }
        }

        private void Start()
        {
            InitializeValues();
            m_stats.StatsChanged += OnStatsChange;
            m_weapon.DamageChange += OnDamageChange;
            m_attackResistance.ResistanceChange += OnResistanceChange;
        }
    }
}