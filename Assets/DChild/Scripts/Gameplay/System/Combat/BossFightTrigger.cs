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
            if (collision.tag != "Sensor")
            {
                var target = collision.GetComponentInParent<ITarget>();
                if (target.CompareTag(Character.objectTag))
                {
                    GameplaySystem.combatManager.MonitorBoss(m_boss);
                    m_boss.SetTarget(collision.GetComponentInParent<Damageable>(), collision.GetComponentInParent<Character>());
                    m_uponTrigger?.Invoke();
                }
            }
        }
    }
}