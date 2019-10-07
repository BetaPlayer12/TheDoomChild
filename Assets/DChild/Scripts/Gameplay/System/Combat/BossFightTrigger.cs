using System;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Event;
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
        [SerializeField]
        private UnityEvent m_onDefeat;

        private void Awake()
        {
            m_boss.GetComponent<Damageable>().Destroyed += OnBossKilled;
        }

        private void OnBossKilled(object sender, EventActionArgs eventArgs)
        {
            m_onDefeat?.Invoke();
        }

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