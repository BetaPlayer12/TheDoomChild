using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonCrashers
{
    public class UIUnitHealthbarBehaviour : MonoBehaviour
    {
        [Header("Unit Health")]
        public UnitHealthBehaviour healthBehaviour;

        [Header("UI References")]
        public UISliderBehaviour healthSlider;

        void OnEnable()
        {
            healthBehaviour.HealthChangedEvent += UpdateHealthDisplay;
        }

        void OnDisable()
        {
            healthBehaviour.HealthChangedEvent -= UpdateHealthDisplay;
        }

        void Start()
        {
            SetupHealthDisplay();
        }

        void SetupHealthDisplay()
        {
            int totalHealth = healthBehaviour.GetCurrentHealth();
            healthSlider.SetupDisplay((float)totalHealth);
            
            UpdateHealthDisplay(totalHealth);
        }

        void UpdateHealthDisplay(int newHealthAmount)
        {
            healthSlider.SetCurrentValue((float)newHealthAmount);
        }

    }
}

