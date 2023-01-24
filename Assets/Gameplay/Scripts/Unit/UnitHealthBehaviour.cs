using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Inspector;

namespace DragonCrashers
{

    public class UnitHealthBehaviour : MonoBehaviour
    {

        [Header("Health Info")]
        [SerializeField]
        [ReadOnly] private int currentHealth;

        [Header("Events")]
        public UnityEvent<int> healthDifferenceEvent;
        public UnityEvent healthIsZeroEvent;

        //Delegate for external systems to detect (IE: Unit's UI)
        public delegate void HealthChangedEventHandler(int newHealthAmount);
        public event HealthChangedEventHandler HealthChangedEvent;


        public void SetupCurrentHealth(int totalHealth)
        {
            currentHealth = totalHealth;
        }

        public void ChangeHealth(int healthDifference)
        {
            currentHealth = currentHealth + healthDifference;

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                HealthIsZeroEvent();
            }

            healthDifferenceEvent.Invoke(healthDifference);
            DelegateEventHealthChanged();
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        void HealthIsZeroEvent()
        {
            healthIsZeroEvent.Invoke();
        }

        void DelegateEventHealthChanged()
        {
            if(HealthChangedEvent != null)
            {
                HealthChangedEvent(currentHealth);
            }
        }

    }
}