using System;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class LockAttackTypeAttribute : Attribute
    {
        private AttackType m_type;

        public LockAttackTypeAttribute()
        {
            m_type = AttackType._COUNT;
        }

        public LockAttackTypeAttribute(AttackType m_type)
        {
            this.m_type = m_type;
        }

        public AttackType type => m_type;
    }
}