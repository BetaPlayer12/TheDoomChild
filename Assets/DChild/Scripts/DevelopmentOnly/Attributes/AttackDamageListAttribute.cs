using System;

namespace DChild.Gameplay.Combat
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AttackDamageListAttribute : Attribute
    {
        private DamageType[] m_types;
        private int m_size;
        private bool m_forceType;

        public AttackDamageListAttribute(params DamageType[] m_types)
        {
            this.m_types = m_types;
            m_size = m_types.Length;
            m_forceType = true;
        }

        public AttackDamageListAttribute(int m_size)
        {
            m_types = null;
            this.m_size = m_size;
            m_forceType = false;
        }

        public DamageType[] types => m_types;
        public int size => m_size;
        public bool forceType => m_forceType;
    }

}