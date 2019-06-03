using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public struct AttackDamage
    {
        [SerializeField]
        private AttackType m_type;
        [SerializeField, MinValue(1)]
        private int m_damage;

        public AttackDamage(AttackType m_type, int m_damage)
        {
            this.m_type = m_type;
            this.m_damage = m_damage;
        }

        public AttackType type { get => m_type; set => m_type = value; }
        public int damage { get => m_damage; set => m_damage = value; }

        public static bool IsMagicAttack(AttackType type)
        {
            return type != AttackType.Physical;
        }

        public static AttackDamage[] Add(AttackDamage[] array, int value)
        {
            var result = new AttackDamage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.damage += value;
                result[i] = data;
            }
            return result;
        }

        public static AttackDamage[] Subtract(AttackDamage[] array, int value)
        {
            var result = new AttackDamage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.damage += value;
                result[i] = data;
            }
            return result;
        }

        public static AttackDamage[] Divide(AttackDamage[] array, int value)
        {
            var result = new AttackDamage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.damage /= value;
                result[i] = data;
            }
            return result;
        }

        public static AttackDamage[] Multiply(AttackDamage[] array, int value)
        {
            var result = new AttackDamage[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var data = array[i];
                data.damage *= value;
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
        public const string ATTACKDAMAGE_VALUE_VARNAME = "m_damage";
    }
}