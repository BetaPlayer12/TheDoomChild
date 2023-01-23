using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonCrashers
{
    public class UIUnitAbilityCooldownBehaviour : MonoBehaviour
    {
        [Header("Ability")]
        public UnitAbilityBehaviour abilityBehaviour; 

        [Header("UI References")]
        public UISliderBehaviour abilityCooldownSlider;
        public GameObject abilityActivateButton;

        void OnEnable()
        {
            abilityBehaviour.AbilityCooldownChangedEvent += UpdateAbilityCooldownDisplay;
            abilityBehaviour.AbilityReadyEvent += EnableAbilityActivateButton;
        }

        void OnDisable()
        {
            abilityBehaviour.AbilityCooldownChangedEvent -= UpdateAbilityCooldownDisplay;
            abilityBehaviour.AbilityReadyEvent -= EnableAbilityActivateButton;
        }

        void Start()
        {
            SetupSliderDisplay();
            ToggleAbilityActivateButton(false);
        }

        void SetupSliderDisplay()
        {
            float totalCooldownTime = abilityBehaviour.data.cooldownTime;
            abilityCooldownSlider.SetupDisplay(totalCooldownTime);
            UpdateAbilityCooldownDisplay(0);
        }

        void UpdateAbilityCooldownDisplay(float newCooldownAmount)
        {
            abilityCooldownSlider.SetCurrentValue(newCooldownAmount);  
        }

        void EnableAbilityActivateButton()
        {
            ToggleAbilityActivateButton(true);
            //abilityBehaviour.AddAbilityToQueue();
        }

        public void AbilityActivateButtonPressed()
        {
            ToggleAbilityActivateButton(false);
            abilityCooldownSlider.SetCurrentValue(0);
            abilityBehaviour.AddAbilityToQueue();
        }

        void ToggleAbilityActivateButton(bool newState)
        {
            abilityActivateButton.SetActive(newState);
        }

        
    }

}
