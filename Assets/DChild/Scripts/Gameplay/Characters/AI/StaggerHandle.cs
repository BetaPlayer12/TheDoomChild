using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public class StaggerHandle
    {
        [System.Serializable]
        public struct Data
        {
            [SerializeField]
            private bool m_isDamageBase;
            [SerializeField, MinValue(1f), ShowIf("m_isDamageBase")]
            private int m_damageThreshold;
            [SerializeField]
            private bool m_isHitCountBase;
            [SerializeField, MinValue(1), ShowIf("m_isHitCountBase")]
            private int m_hitCountThreshold;

            public int damageThreshold => m_isDamageBase ? m_damageThreshold : -1;
            public int hitCountThreshold => m_isHitCountBase ? m_hitCountThreshold : -1;
        }

        private Data m_currentData;

        private int m_damageLeft;
        private int m_hitLeft;
        private Damageable m_source;

        public Action React;

        public StaggerHandle(Damageable source,Data data, Action reaction)
        {
            m_source = source;
            m_source.DamageTaken += Execute;

            m_currentData = data;
            m_damageLeft = m_currentData.damageThreshold;
            m_hitLeft = m_currentData.hitCountThreshold;

            React = reaction;
        }

        public void SetData(Data data) => m_currentData = data;

        public void Reset()
        {
            m_damageLeft = m_currentData.damageThreshold;
            m_hitLeft = m_currentData.hitCountThreshold;
        }

        public void SetReaction(Action reaction)
        {
            React = reaction;
        }

        public void ConnectTo(Damageable damageable)
        {
            if (m_source != null)
            {
                m_source.DamageTaken -= Execute;
                m_source = null;
            }

            m_source = damageable;
            m_source.DamageTaken += Execute;
        }

        private void Execute(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if(m_damageLeft > 0)
            {
                m_damageLeft -= eventArgs.damage;
                if(m_damageLeft < 0)
                {
                    React?.Invoke();
                    Reset();
                    return;
                }
            }

            if(m_hitLeft > 0)
            {
                m_hitLeft -= 1;
                if(m_hitLeft == 0)
                {
                    React?.Invoke();
                    Reset();
                    return;
                }
            }
        }
    }
}