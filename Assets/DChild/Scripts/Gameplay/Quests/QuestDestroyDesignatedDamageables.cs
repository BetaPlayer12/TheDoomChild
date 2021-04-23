using UnityEngine;
using DChild.Gameplay.Combat;
using Holysoft.Event;

namespace DChild.Gameplay.Quests
{
    public class QuestDestroyDesignatedDamageables : QuestAdvancer
    {
        [SerializeField]
        private Damageable[] m_toDestroy;

        private int m_aliveCount;
        private void OnObjectDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_aliveCount--;
            if (m_aliveCount <= 0)
            {
                ExecuteLuaScript();
            }
        }

        public void Reset()
        {
            m_aliveCount = m_toDestroy.Length;
            for (int i = 0; i < m_toDestroy.Length; i++)
            {
                m_toDestroy[i].Heal(999_999);
            }
        }

        private void Awake()
        {
            for (int i = 0; i < m_toDestroy.Length; i++)
            {
                m_toDestroy[i].Destroyed += OnObjectDestroyed;
            }
            m_aliveCount = m_toDestroy.Length;
        }

    }
}