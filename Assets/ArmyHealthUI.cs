using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyHealthUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_armyPower;
        public void MonitorHealth(Army army)
        {
            //Set Cuurent UI to Count without anim
            army.OnTroopCountChange += OnTroopCountChange;
        }

        private void OnTroopCountChange(object sender, Army.TroopCountChangeEventArgs eventArgs)
        {
            m_armyPower.text = $"{eventArgs.currentTroopCount}";
        }
    }
}