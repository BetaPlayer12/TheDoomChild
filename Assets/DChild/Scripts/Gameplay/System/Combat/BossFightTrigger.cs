using DChild.Gameplay.Characters.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Combat
{
    public class BossFightTrigger : MonoBehaviour
    {
        [SerializeField]
        private Boss m_boss;
        [SerializeField]
        private UnityEvent m_uponTrigger;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameplaySystem.combatManager.MonitorBoss(m_boss);
            //m_boss.SetTarget();
            m_uponTrigger?.Invoke();
        }
    }
}