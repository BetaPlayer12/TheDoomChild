using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using Utilities.Inspector;

namespace DragonCrashers
{

    public class UnitAbilityBehaviour : MonoBehaviour
    {
        public enum QueueActionAfterCooldown
        {
            Automatic,
            ManualButton
        }

        [Header("Settings")]
        public UnitAbilityData data;
        public QueueActionAfterCooldown queueActionAfterCooldown;

        [Header("Runtime ID")]
        [SerializeField]
        [ReadOnly] private int ID;

        //Cooldown Timer
        public DurationTimer cooldownTimer;

        [Header("States")]

        [SerializeField]
        [ReadOnly] public bool cooldownActive;

        [SerializeField]
        [ReadOnly] public bool abilityReady;

        private bool waitToBeAddedToQueue;

        [Header("Ability Timeline")]
        public PlayableDirector abilityTimeline;

        [Header("Events")]
        public UnityEvent<int> abilityReadyForQueue;
        public UnityEvent<int, TargetType> applyAbilityValueToTargets;
        public UnityEvent<int> abilitySequenceFinished;

        //Delegate for external systems to detect (IE: Unit's UI)
        public delegate void AbilityCooldownChangedEventHandler(float newCooldownAmount);
        public event AbilityCooldownChangedEventHandler AbilityCooldownChangedEvent;

        public delegate void AbilityReadyEventHandler();
        public event AbilityReadyEventHandler AbilityReadyEvent;


        public void SetupID(int newID)
        {
            ID = newID;
        }

        public void SetupAbilityCooldownTimer()
        {
            cooldownTimer = new DurationTimer(data.cooldownTime);
        }

        public void StartAbilityCooldown()
        {
            cooldownActive = true;
            abilityReady = false;
        }

        void Update()
        {
            CheckAbilityCooldown();
        }

        void CheckAbilityCooldown()
        {
            if(cooldownActive)
            {
                cooldownTimer.UpdateTimer();

                DelegateEventAbilityCooldownChanged();
                
                if(cooldownTimer.HasElapsed())
                {
                    cooldownTimer.EndTimer();
                    cooldownTimer.Reset();
                    AbilityCooldownFinished();
                    return;
                }
                
            }
        }

        void AbilityCooldownFinished()
        {
            cooldownActive = false;
            abilityReady = true;

            switch(queueActionAfterCooldown)
            {
                case QueueActionAfterCooldown.Automatic:
                    AddAbilityToQueue();
                    break;

                case QueueActionAfterCooldown.ManualButton:
                    DelegateEventAbilityReady();
                    break;      
            }

        }

        public void AddAbilityToQueue()
        {
            if(abilityReady)
            {
                abilityReadyForQueue.Invoke(ID);
            } 
        }

        public void BeginAbilitySequence()
        {
            abilityTimeline.Play();
            abilityReady = false;
        }

        public void AbilityMarkerHappened()
        {
            int abilityValue = data.GetRandomValueInRange();
            applyAbilityValueToTargets.Invoke(abilityValue, data.targetType);
        }

        public void AbilitySequenceFinished()
        {
            cooldownActive = true;
            abilitySequenceFinished.Invoke(ID);
        }

        public void StopAbility()
        {
            cooldownActive = false;
            abilityReady = false;
        }

        void DelegateEventAbilityCooldownChanged()
        {
            if(AbilityCooldownChangedEvent != null)
            {
                AbilityCooldownChangedEvent(cooldownTimer.GetPolledTime());
            }
        }

        void DelegateEventAbilityReady()
        {
            if(AbilityReadyEvent != null)
            {
                AbilityReadyEvent();
            }
        }
   
    }
}