using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public class PercentHealthPhaseConditionTemplate<T> : IPhaseConditionTemplate<T> where T : System.Enum
    {
        [SerializeField, Tooltip("Please Restrict Value to 0-100, And make sure no two values are the same"), HideReferenceObjectPicker]
        private Dictionary<T, int> m_info = new Dictionary<T, int>();

        public IPhaseConditionHandle<T> CreateHandle(Character character)
        {
            return new PercentHealthPhaseConditionHandle<T>(character.GetComponentInChildren<Health>(), m_info);
        }
    }

    public class PercentHealthPhaseConditionHandle<T> : IPhaseConditionHandle<T> where T : System.Enum
    {
        private struct Info
        {
            public Info(T phase, int healthThreshold) : this()
            {
                this.phase = phase;
                this.healthThreshold = healthThreshold;
            }

            public T phase { get; }
            public int healthThreshold { get; }
        }

        private Health m_health;
        private Info[] m_infoList;

        public PercentHealthPhaseConditionHandle(Health health, Dictionary<T, int> pairs)
        {
            this.m_health = health;
            List<Info> list = new List<Info>();
            var maxHealth = m_health.maxValue;
            foreach (var key in pairs.Keys)
            {
                var healthThreshold = Mathf.FloorToInt(maxHealth * (pairs[key] / 100f));
                list.Add(new Info(key, healthThreshold));
            }
            list.Sort((x, y) => x.healthThreshold.CompareTo(y.healthThreshold));
            m_infoList = list.ToArray();
        }

        public T GetProposedPhase()
        {
            for (int i = 0; i < m_infoList.Length; i++)
            {
                if (m_health.currentValue <= m_infoList[i].healthThreshold)
                {
                    return m_infoList[i].phase;
                }
            }

            return (T)Enum.ToObject(typeof(T), 0);
        }

    }
}