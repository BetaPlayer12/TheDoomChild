using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using static DChild.Gameplay.Systems.WeaponUpgradeInfo;

namespace DChild.Gameplay.Characters.Players
{

    [AddComponentMenu("DChild/Gameplay/Player/Player Weapon")]
    [System.Serializable]
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField]
        private WeaponBaseStatsData m_defaultWeaponStatsData;

        [SerializeField]
        private WeaponLevel m_currentWeaponLevel;
        public WeaponLevel currentWeaponLevel => m_currentWeaponLevel;

        [HorizontalGroup("Split")]

        [SerializeField, OnValueChanged("UpdateDamage", true), BoxGroup("Split/Base"), HideLabel]
        private Damage m_baseDamage;
        [ShowInInspector, HideInEditorMode, OnValueChanged("UpdateDamage", true),
        BoxGroup("Split/Added"), HideLabel]
        private int m_addedDamageValue;
        [ShowInInspector, HideInEditorMode, ReadOnly, BoxGroup("Total"), HideLabel]
        private Damage m_totalDamage;
        private bool m_isInitialized;
        [ShowInInspector, HideInEditorMode, BoxGroup("Status Inflictions"), HideLabel]
        private List<StatusEffectChance> m_statusInflictions;

        public Damage damage => m_totalDamage;
        public List<StatusEffectChance> statusInflictions => m_statusInflictions;

        public event EventAction<EventActionArgs> DamageChange;
        public event EventAction<EventActionArgs> StatusInflictionUpdate;

        public void Initialize()
        {          
            if (m_isInitialized == false)
            {
                m_baseDamage = m_defaultWeaponStatsData.damage;

                CalculateTotalDamage();
                m_statusInflictions = new List<StatusEffectChance>(m_defaultWeaponStatsData.statusInflictions);
                m_isInitialized = true;
            }
        }
        public void SetAddedDamage(int damage)
        {
            m_addedDamageValue = damage;
            CalculateTotalDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetBaseDamage(Damage damage)
        {
            m_baseDamage = damage;
            CalculateTotalDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }
        public void SetWeaponLevel(WeaponLevel level)
        {
            m_currentWeaponLevel = level;
        }

        public WeaponLevel GetWeaponLevel()
        {
            return m_currentWeaponLevel;
        }

        public void SetInfliction(StatusEffectType type, int chanceValue)
        {
            chanceValue = Mathf.Clamp(chanceValue, 0, 100);
            if (chanceValue == 0)
            {
                if (Contains(type, out int index))
                {
                    m_statusInflictions.RemoveAt(index);
                }
            }
            else
            {
                if (Contains(type, out int index))
                {
                    var info = m_statusInflictions[index];
                    info.chance = chanceValue;
                    m_statusInflictions[index] = info;
                }
                else
                {
                    m_statusInflictions.Add(new StatusEffectChance(type, chanceValue));
                }
            }
            StatusInflictionUpdate?.Invoke(this, EventActionArgs.Empty);
        }

        private bool Contains(StatusEffectType type, out int index)
        {
            for (int i = 0; i < m_statusInflictions.Count; i++)
            {
                if (m_statusInflictions[i].type == type)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }


        private void CalculateTotalDamage()
        {
            m_totalDamage = m_baseDamage;
            m_totalDamage.value += m_addedDamageValue;
        }

        public WeaponUpgradeSaveData SaveData()
        {
            return new WeaponUpgradeSaveData(m_currentWeaponLevel);
        }

        public void LoadData(WeaponUpgradeSaveData upgradeSaveData)
        {
            m_currentWeaponLevel = upgradeSaveData.currentWeaponLevel;
        }
#if UNITY_EDITOR
        private void UpdateDamage()
        {
            CalculateTotalDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }
#endif
    }
}