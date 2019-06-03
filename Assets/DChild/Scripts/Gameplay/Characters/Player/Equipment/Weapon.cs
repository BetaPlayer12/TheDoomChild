using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusInfliction;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Equipments
{
    [System.Serializable]
    public class Weapon : IWeapon
    {
        [SerializeField, LabelText("Weapon Damages")]
        private List<AttackDamage> m_damageList;
        [SerializeField, LabelText("Status Effects")]
        private List<StatusInflictionInfo> m_statusToInflict;

        public event EventAction<EventActionArgs> ValueChanged;

        public AttackDamage[] damageList => m_damageList.ToArray();
        public bool canInflictStatusEffects => m_statusToInflict.Count > 0;
        public StatusInflictionInfo[] statusToInflict => m_statusToInflict.ToArray();


        public void AddDamage(AttackType type, int value)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var damageElement = m_damageList[index];
                damageElement.damage += value;
                m_damageList[index] = damageElement;
            }
            else
            {
                m_damageList.Add(new AttackDamage(type, value));
            }
            ValueChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public void ReduceDamage(AttackType type, int value)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var damageElement = m_damageList[index];
                damageElement.damage -= value;
                if (damageElement.damage <= 0)
                {
                    m_damageList.RemoveAt(index);
                }
                else
                {

                    m_damageList[index] = damageElement;
                }
                ValueChanged?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void AddStatusInfliction(StatusEffectType type, float chance)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var statusInfliction = m_statusToInflict[index];
                statusInfliction.chance += chance;
                m_statusToInflict[index] = statusInfliction;
            }
            else
            {
                m_statusToInflict.Add(new StatusInflictionInfo(type, chance));
            }
        }

        public void ReduceStatusInfliction(StatusEffectType type, float chance)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var statusInfliction = m_statusToInflict[index];
                statusInfliction.chance -= chance;
                if (statusInfliction.chance <= 0)
                {
                    m_statusToInflict.RemoveAt(index);
                }
                else
                {
                    m_statusToInflict[index] = statusInfliction;
                }
            }
        }

        private int FindIndex(AttackType type)
        {
            for (int i = 0; i < m_damageList.Count; i++)
            {
                if (m_damageList[i].type == type)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindIndex(StatusEffectType type)
        {
            for (int i = 0; i < m_statusToInflict.Count; i++)
            {
                if (m_statusToInflict[i].effect == type)
                {
                    return i;
                }
            }
            return -1;
        }

#if UNITY_EDITOR
        public void Initialize(params AttackDamage[] damages)
        {
            m_damageList = new List<AttackDamage>(damages);
        }


#endif
    }
}