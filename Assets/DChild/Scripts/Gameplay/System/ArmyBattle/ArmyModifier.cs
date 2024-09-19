using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public struct ArmyModifier 
    {
        public ArmyDamageTypeModifier damageModifier;
        public ArmyDamageTypeModifier resistanceModifier;

        public void Reset()
        {
            damageModifier.ResetModifiers();
            resistanceModifier.ResetModifiers();
        }
    }
}

