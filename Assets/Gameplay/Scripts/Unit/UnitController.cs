using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Inspector;

namespace DragonCrashers
{

    public class UnitController : MonoBehaviour
    {
        
        [Header("Data")]
        public UnitInfoData data;
        
        [Header("Health Settings")]
        public UnitHealthBehaviour healthBehaviour;
        private bool unitIsAlive;

        [Header("Target Settings")]
        public UnitTargetsBehaviour targetsBehaviour;

        [Header("Ability Settings")]
        public UnitAbilitiesBehaviour abilitiesBehaviour;

        [Header("Animation Settings")]
        public UnitCharacterAnimationBehaviour characterAnimationBehaviour;

        [Header("Audio Settings")]
        public UnitAudioBehaviour audioBehaviour;

        [Header("Debug")]
        public bool initializeSelf;
        
        public delegate void UnitDiedEventHandler(UnitController unit);
        public event UnitDiedEventHandler UnitDiedEvent;

        void Start()
        {
            if(initializeSelf)
            {
                SetAlive();
                BattleStarted();
            }
        }
        
        public void AssignTargetUnits(List<UnitController> units)
        {
            targetsBehaviour.AddTargetUnits(units);
        }

        public void RemoveTargetUnit(UnitController unit)
        {
            targetsBehaviour.RemoveTargetUnit(unit);
        }

        public void SetAlive()
        {
            healthBehaviour.SetupCurrentHealth(data.totalHealth);
            unitIsAlive = true;
        }

        public void BattleStarted()
        {
            abilitiesBehaviour.StartAbilityCooldowns();
        }

        public void BattleEnded()
        {
            abilitiesBehaviour.StopAllAbilities();
        }

        public void AbilityHappened(int abilityValue, TargetType unitTargetType)
        {   
            List<UnitController> targetUnits = targetsBehaviour.FilterTargetUnits(unitTargetType);

            if(targetUnits.Count > 0)
            {
                for(int i = 0; i < targetUnits.Count; i++)
                {
                    targetUnits[i].RecieveAbilityValue(abilityValue);
                }  
            } 
            
        }

        public void RecieveAbilityValue(int abilityValue)
        {
            if(unitIsAlive)
            {
                healthBehaviour.ChangeHealth(abilityValue);
                characterAnimationBehaviour.CharacterWasHit();
                audioBehaviour.PlaySFXGetHit();
            }
        }
        
        public int GetCurrentHealth()
        {
            return healthBehaviour.GetCurrentHealth();
        }

        public void UnitHasDied()
        {
            unitIsAlive = false;
            abilitiesBehaviour.StopAllAbilities();
            characterAnimationBehaviour.CharacterHasDied();
            audioBehaviour.PlaySFXDeath();

            DelegateEventUnitDied();
        }

        void DelegateEventUnitDied()
        {
            if(UnitDiedEvent != null)
            {
                UnitDiedEvent(this);
            }
        }
        
    }
}