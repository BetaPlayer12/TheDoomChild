using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public struct Damage
    {
        [SerializeField,EnumPaging]
        private DamageType m_type;
        [SerializeField, MinValue(0)]
        private int m_value;

        public Damage(DamageType type, int value)
        {
            this.m_type = type;
            this.m_value = value;
        }

        public DamageType type { get => m_type; set => m_type = value; }
        public int value { get => m_value; set => m_value = value; }

        public static bool IsMagicDamage(DamageType type)
        {
            return type != DamageType.Physical;
        }

        public static Damage[] Add(Damage[] array, int value)
        {
            var result = new Damage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.value += value;
                result[i] = data;
            }
            return result;
        }

        public static Damage[] Subtract(Damage[] array, int value)
        {
            var result = new Damage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.value += value;
                result[i] = data;
            }
            return result;
        }

        public static Damage[] Divide(Damage[] array, int value)
        {
            var result = new Damage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.value /= value;
                result[i] = data;
            }
            return result;
        }

        public static Damage[] Multiply(Damage[] array, int value)
        {
            var result = new Damage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.value *= value;
                result[i] = data;
            }
            return result;
        }
    }
}

namespace DChildEditor
{
    public static partial class Convention
    {
        public const string DAMAGE_VALUE_VARNAME = "m_value";
    }
}