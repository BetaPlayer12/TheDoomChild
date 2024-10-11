using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [AddComponentMenu("DChild/Gameplay/Combat/Status Inflictor")]
    public class StatusInflictor : MonoBehaviour
    {
        [SerializeField]
        private StatusEffectChanceData m_data;
        [ShowInInspector, HideInEditorMode]
        private List<StatusEffectChance> m_statusInflictions;

        public void SetInflictionList(IReadOnlyList<StatusEffectChance> list)
        {
            m_statusInflictions.Clear();
            m_statusInflictions.AddRange(list);
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
        }

        public void SetData(StatusEffectChanceData data)
        {
            if (m_data != data)
            {
                m_data = data;
                CopyData();
            }
        }

        public void InflictStatusTo(StatusEffectReciever reciever)
        {
           GameplaySystem.combatManager.Inflict(reciever, m_statusInflictions.ToArray());
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

        private void CopyData()
        {
            if (m_statusInflictions != null)
            {
                m_statusInflictions.Clear();
                var chances = m_data.chance;
                foreach (var key in chances.Keys)
                {
                    m_statusInflictions.Add(new StatusEffectChance(key, chances[key]));
                }
            }
        }

        private void OnTargetDamage(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (m_statusInflictions.Count > 0)
            {
                if (eventArgs.target.instance.isAlive && eventArgs.target.statusEffectReciever != null)
                {
                    GameplaySystem.combatManager.Inflict(eventArgs.target.statusEffectReciever, m_statusInflictions.ToArray());
                }

            }
        }

        private void Awake()
        {
            m_statusInflictions = new List<StatusEffectChance>();
            if (m_data != null)
            {
                CopyData();
            }
            GetComponent<IAttacker>().TargetDamaged += OnTargetDamage;
        }
    }
}