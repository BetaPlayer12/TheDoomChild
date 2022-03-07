using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [AddComponentMenu("DChild/Gameplay/Combat/Status Effect Resistance")]
    public class StatusEffectResistance : SerializedMonoBehaviour, IStatusEffectResistance
    {
        public struct ResistanceEventArgs : IEventActionArgs
        {
            public ResistanceEventArgs(StatusEffectType type, int value) : this()
            {
                this.type = type;
                this.value = value;
            }

            public StatusEffectType type { get; }
            public int value { get; }
        }

        [SerializeField]
        private StatusEffectChanceData m_data;
        [ShowInInspector, HideInEditorMode]
        private Dictionary<StatusEffectType, int> m_resistances = new Dictionary<StatusEffectType, int>();

        public event EventAction<ResistanceEventArgs> ResistanceChange;

        public int GetResistance(StatusEffectType type) => m_resistances.ContainsKey(type) ? m_resistances[type] : 0;

        public void SetResistanceList(Dictionary<StatusEffectType, int> list)
        {
            Copy(list, m_resistances);
            ResistanceChange?.Invoke(this, new ResistanceEventArgs(StatusEffectType._COUNT,0));
        }

        public void SetResistance(StatusEffectType type, int resistanceValue)
        {
            resistanceValue = Mathf.Clamp(resistanceValue, 0, 100);
            if (resistanceValue == 0)
            {
                if (m_resistances.ContainsKey(type))
                {
                    m_resistances.Remove(type);
                }
            }
            else
            {
                if (m_resistances.ContainsKey(type))
                {
                    m_resistances[type] = resistanceValue;
                }
                else
                {
                    m_resistances.Add(type, resistanceValue);
                }
            }
            ResistanceChange?.Invoke(this, new ResistanceEventArgs(type, resistanceValue));
        }

        public void SetData(StatusEffectChanceData data)
        {
            if (m_data != data)
            {
                m_data = data;
                if (m_resistances != null)
                {
                    Copy(m_data.chance, m_resistances);
                    ResistanceChange?.Invoke(this, new ResistanceEventArgs(StatusEffectType._COUNT, 0));
                }
            }
        }

        public void Initialize()
        {
            m_resistances = new Dictionary<StatusEffectType, int>();
            if (m_data != null)
            {
                Copy(m_data.chance, m_resistances);
            }
        }

        private void Copy(Dictionary<StatusEffectType, int> source, Dictionary<StatusEffectType, int> destination)
        {
            destination.Clear();
            foreach (var key in source.Keys)
            {
                destination.Add(key, source[key]);
            }
        }
    }
}