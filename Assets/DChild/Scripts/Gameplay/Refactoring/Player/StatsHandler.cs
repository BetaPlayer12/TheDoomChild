using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public class StatsHandler : MonoBehaviour
    {
        [SerializeField]
        private PlayerAttributes m_attributes;
        [SerializeField]
        private PlayerStats m_stats;
        [SerializeField]
        private StatBonusHandle m_bonusHandle;
        [SerializeField]
        private PlayerWeapon m_weapon;

        private PlayerStatsInfo m_bonusFromAttributes;
        private bool m_updateDamage;

        private void Awake()
        {
            UpdateBonusStats();
            m_attributes.ValueChange += OnAttributeChange;
            m_stats.StatsChanged += OnStatsChange;
            m_weapon.Initialize();
            UpdateDamage();
            m_weapon.BaseDamageChange += OnBaseDamageChange;
        }

        private void OnStatsChange(object sender, StatValueEventArgs eventArgs)
        {
            if (eventArgs.stat == PlayerStat._COUNT)
            {
                UpdateDamage();
            }
            else if (m_updateDamage == false)
            {
                m_updateDamage = eventArgs.stat == PlayerStat.Attack || eventArgs.stat == PlayerStat.MagicAttack;
            }
        }

        private void OnBaseDamageChange(object sender, EventActionArgs eventArgs)
        {
            UpdateDamage();
        }

        private void OnAttributeChange(object sender, AttributeValueEventArgs eventArgs)
        {
            m_updateDamage = false;
            UpdateBonusStats();
            if (m_updateDamage)
            {
                UpdateDamage();
            }
        }

        private void UpdateBonusStats()
        {
            var statCount = (int)PlayerStat._COUNT;
            for (int i = 0; i < statCount; i++)
            {
                var stat = (PlayerStat)i;
                if (m_bonusHandle.HasBonus(stat))
                {
                    m_stats.AddStat(stat, -m_bonusFromAttributes.GetStat(stat));
                    var bonusValue = m_bonusHandle.GetBonus(stat, m_attributes);
                    m_bonusFromAttributes.SetStat(stat, bonusValue);
                    m_stats.AddStat(stat, bonusValue);
                }
            }
        }

        private void UpdateDamage()
        {
            var damages = m_weapon.baseDamage;
            for (int i = 0; i < damages.Length; i++)
            {
                var damage = damages[i];
                if (AttackDamage.IsMagicAttack(damages[i].type))
                {
                    damage.damage += m_stats.GetStat(PlayerStat.MagicAttack);
                }
                else
                {
                    damage.damage += m_stats.GetStat(PlayerStat.Attack);
                }
                damages[i] = damage;
            }
            m_weapon.SetTotalDamage(damages);
        }
    }
}