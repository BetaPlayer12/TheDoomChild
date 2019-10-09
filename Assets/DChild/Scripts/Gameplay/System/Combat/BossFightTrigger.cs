using System;
using System.Collections;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Combat
{
    public class BossFightTrigger : MonoBehaviour
    {
        [SerializeField]
        private Boss m_boss;
        [SerializeField, MinValue(0)]
        private float m_startDelay;
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

        private IEnumerator DelayedAwakeRoutine(Damageable damageable, Character character)
        {
            yield return new WaitForSeconds(m_startDelay);
            m_boss.SetTarget(damageable, character);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor")
            {
                var target = collision.GetComponentInParent<ITarget>();
                if (target.CompareTag(Character.objectTag))
                {
                    GameplaySystem.combatManager.MonitorBoss(m_boss);
                    if (m_startDelay == 0)
                    {
                        m_boss.SetTarget(collision.GetComponentInParent<Damageable>(), collision.GetComponentInParent<Character>());
                    }
                    else
                    {
                        StartCoroutine(DelayedAwakeRoutine(collision.GetComponentInParent<Damageable>(), collision.GetComponentInParent<Character>()));
                    }
                    m_uponTrigger?.Invoke();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}