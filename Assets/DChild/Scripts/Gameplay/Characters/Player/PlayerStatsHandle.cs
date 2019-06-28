using DChild.Gameplay.Combat;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayerStats
    {
        event EventAction<StatValueEventArgs> StatsChanged;
        void AddStat(PlayerStat stat, int value);
        void ApplyChanges();
        int GetStat(PlayerStat stat);
    }

    public struct StatValueEventArgs : IEventActionArgs
    {
        public StatValueEventArgs(PlayerStat stat, int value) : this()
        {
            this.stat = stat;
            this.value = value;
        }

        public PlayerStat stat { get; }
        public int value { get; }
    }

    [System.Serializable]
    public class PlayerStatsHandle : IPlayerStats
    {
        [SerializeField]
        private StatBonusHandle m_bonusHandle;
        private IAttributes m_attributes;
        private IEquipment m_equipment;
        private IMaxStat m_health;
        private IMaxStat m_magic;
        [ShowInInspector, HideInEditorMode]
        private PlayerStatsInfo m_totalStats;
        private PlayerStatsInfo m_addedStats;

        public event EventAction<StatValueEventArgs> StatsChanged;

        public AttackDamage[] damages { get; private set; }

        private int baseCritChance => 25;
        private int baseStatusChance => 35;

        private List<PlayerStat> m_changesToApply;

        public void Initialize(IAttributes attributes, IEquipment equipment, IMaxStat health, IMaxStat magic)
        {
            m_equipment = equipment;
            if (m_equipment != null)
            {
                damages = m_equipment.weapon.damageList;
                m_equipment.weapon.ValueChanged += OnWeaponDamageChange;

                m_addedStats = new PlayerStatsInfo();
                m_totalStats = new PlayerStatsInfo();
                m_totalStats.SetStat(PlayerStat.Defense, m_equipment.armor.defense);
                m_totalStats.SetStat(PlayerStat.MagicDefense, m_equipment.armor.magicDefense);
                m_totalStats.SetStat(PlayerStat.CritChance, baseCritChance);
                m_totalStats.SetStat(PlayerStat.StatusChance, baseStatusChance);
            }

            m_health = health;
            m_health?.SetMaxValue(100);
            m_magic = magic;
            m_magic?.SetMaxValue(100);

            m_attributes = attributes;
            if (m_attributes != null)
            {
                ApplyAttributes();
                SubscribeListeners();
            }
            m_changesToApply = new List<PlayerStat>();
        }

        public void SubscribeListeners() => m_attributes.ValueChange += OnAttributeChange;

        public void UnsubscribeListeners() => m_attributes.ValueChange += OnAttributeChange;

        public void AddStat(PlayerStat stat, int value)
        {
            m_addedStats.AddStat(stat, value);
            if (m_changesToApply.Contains(stat) == false)
            {
                m_changesToApply.Add(stat);
            }
        }

        [Button, HideInEditorMode]
        public void ApplyChanges()
        {
            for (int i = 0; i < m_changesToApply.Count; i++)
            {
                UpdateStat(m_changesToApply[i]);
            }
            m_changesToApply.Clear();
        }

        public int GetStat(PlayerStat stat) => m_totalStats.GetStat(stat);

        public void ApplyAttributes()
        {
            UpdateStat(PlayerStat.Health);
            UpdateStat(PlayerStat.Magic);
            UpdateStat(PlayerStat.Defense);
            UpdateStat(PlayerStat.MagicDefense);
            UpdateStat(PlayerStat.Attack);
            UpdateStat(PlayerStat.MagicAttack);
            UpdateStat(PlayerStat.CritChance);
            UpdateStat(PlayerStat.StatusChance);
            UpdateDamage();
        }

        private void OnAttributeChange(object sender, AttributeValueEventArgs eventArgs)
        {
            switch (eventArgs.atttribute)
            {
                case Attribute.Vitality:
                    UpdateStat(PlayerStat.Health);
                    UpdateStat(PlayerStat.Defense);
                    UpdateStat(PlayerStat.MagicDefense);
                    break;
                case Attribute.Intelligence:
                    UpdateStat(PlayerStat.Magic);
                    UpdateStat(PlayerStat.MagicDefense);
                    UpdateStat(PlayerStat.MagicAttack);
                    UpdateDamage();
                    break;
                case Attribute.Strength:
                    UpdateStat(PlayerStat.Attack);
                    UpdateDamage();
                    break;
                case Attribute.Luck:
                    UpdateStat(PlayerStat.CritChance);
                    UpdateStat(PlayerStat.StatusChance);
                    break;
            }
        }

        private void OnWeaponDamageChange(object sender, EventActionArgs eventArgs)
        {
            UpdateDamage();
        }

        private void UpdateStat(PlayerStat stat)
        {
            var newValue = m_bonusHandle.GetBonus(stat, m_attributes) + m_addedStats.GetStat(stat);
            m_totalStats.SetStat(stat, newValue);
            switch (stat)
            {
                case PlayerStat.Health:
                    m_health?.SetMaxValue(m_totalStats.GetStat(PlayerStat.Health));
                    break;
                case PlayerStat.Magic:
                    m_magic?.SetMaxValue(m_totalStats.GetStat(PlayerStat.Magic));
                    break;
                case PlayerStat.Attack:
                case PlayerStat.MagicAttack:
                    UpdateDamage();
                    break;
                case PlayerStat.Defense:
                    m_totalStats.AddStat(PlayerStat.Defense, m_equipment.armor.defense);
                    break;
                case PlayerStat.MagicDefense:
                    m_totalStats.AddStat(PlayerStat.MagicDefense, m_equipment.armor.magicDefense);
                    break;
            }
            StatsChanged?.Invoke(this, new StatValueEventArgs(stat, m_totalStats.GetStat(stat)));
        }

        private void UpdateDamage()
        {
            var weaponDamages = m_equipment.weapon.damageList;
            damages = new AttackDamage[weaponDamages.Length];
            for (int i = 0; i < damages.Length; i++)
            {
                if (AttackDamage.IsMagicAttack(weaponDamages[i].type))
                {
                    damages[i] = new AttackDamage(weaponDamages[i].type, weaponDamages[i].damage + m_totalStats.GetStat(PlayerStat.MagicAttack));
                }
                else
                {
                    damages[i] = new AttackDamage(weaponDamages[i].type, weaponDamages[i].damage + m_totalStats.GetStat(PlayerStat.Attack));
                }
            }
        }
    }
}