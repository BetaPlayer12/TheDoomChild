using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public class PlayerWeapon : MonoBehaviour
    {
        [HorizontalGroup("Split")]

        [SerializeField, OnValueChanged("UpdateDamage", true), BoxGroup("Split/Base"), HideLabel]
        private AttackDamage m_baseDamage;
        [ShowInInspector, HideInEditorMode, OnValueChanged("UpdateDamage", true),
        BoxGroup("Split/Added"), HideLabel]
        private AttackDamage m_addedDamage;
        [ShowInInspector, HideInEditorMode, ReadOnly, BoxGroup("Total"), HideLabel]
        private List<AttackDamage> m_combinedDamage;
        private bool m_isInitialized;
        [ShowInInspector, HideInEditorMode, BoxGroup("Status Inflictions"), HideLabel]
        private List<StatusEffectChance> m_statusInflictions;

        public AttackDamage[] damage { get => m_combinedDamage.ToArray(); }
        public List<StatusEffectChance> statusInflictions => m_statusInflictions;

        public event EventAction<EventActionArgs> DamageChange;
        public event EventAction<EventActionArgs> StatusInflictionUpdate;

        public void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_combinedDamage = new List<AttackDamage>();
                CalculateCombinedDamage();
                m_statusInflictions = new List<StatusEffectChance>();
                m_isInitialized = true;
            }
        }
        public void SetAddedDamage(AttackDamage damage)
        {
            m_addedDamage = damage;
            CalculateCombinedDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetBaseDamage(AttackDamage damage)
        {
            m_baseDamage = damage;
            CalculateCombinedDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetInfliction(StatusEffectType type, int resistanceValue)
        {
            resistanceValue = Mathf.Clamp(resistanceValue, 0, 100);
            if (resistanceValue == 0)
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
                    info.chance = resistanceValue;
                    m_statusInflictions[index] = info;
                }
                else
                {
                    m_statusInflictions.Add(new StatusEffectChance(type, resistanceValue));
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

        private void CalculateCombinedDamage()
        {
            m_combinedDamage.Clear();
            m_combinedDamage.Add(m_baseDamage);
            if (m_addedDamage.value > 0)
            {
                m_combinedDamage.Add(m_addedDamage);
            }
        }

        private void Awake()
        {
            Initialize();
        }

#if UNITY_EDITOR
        private void UpdateDamage()
        {
            CalculateCombinedDamage();
            DamageChange?.Invoke(this, EventActionArgs.Empty);
        }
#endif
    }
}