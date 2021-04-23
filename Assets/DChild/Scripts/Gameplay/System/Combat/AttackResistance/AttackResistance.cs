using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public abstract class AttackResistance : SerializedMonoBehaviour, IAttackResistance
    {
        public struct ResistanceEventArgs : IEventActionArgs
        {
            public ResistanceEventArgs(AttackType type, float resistanceValue) : this()
            {
                this.type = type;
                this.resistanceValue = resistanceValue;
            }

            public AttackType type { get; }
            public float resistanceValue { get; }
        }
        protected abstract Dictionary<AttackType, float> resistance { get; }
        public event EventAction<ResistanceEventArgs> ResistanceChange;

        public abstract void SetData(AttackResistanceData data);
        public abstract void ClearResistance();
        public abstract void SetResistance(AttackType type, float resistanceValue);
        public float GetResistance(AttackType type) => resistance.ContainsKey(type) ? resistance[type] : 0;

        protected void CallResistanceChange(ResistanceEventArgs eventArgs) => ResistanceChange?.Invoke(this, eventArgs);

        protected void SetResistance(Dictionary<AttackType, float> list, AttackType type, float resistanceValue)
        {
            if (resistanceValue == 0)
            {
                if (list.ContainsKey(type))
                {
                    list.Remove(type);
                }
            }
            else
            {
                if (list.ContainsKey(type))
                {
                    list[type] = resistanceValue;
                }
                else
                {
                    list.Add(type, resistanceValue);
                }
            }
        }

        protected static float ConvertToFloat(AttackResistanceType type)
        {
            switch (type)
            {
                case AttackResistanceType.None:
                    return 0;
                case AttackResistanceType.Weak:
                    return -1;
                case AttackResistanceType.Strong:
                    return 0.5f;
                case AttackResistanceType.Immune:
                    return 1;
                case AttackResistanceType.Absorb:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}