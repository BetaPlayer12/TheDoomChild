using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonCrashers
{
    public class UnitDamageDisplayBehaviour : MonoBehaviour
    {

        [Header("Damage Color Tint")]
        public Color damageColorTint;

        [Header("Damage Location")]
        public Transform damageDisplayTransform;

        public delegate void DamageDisplayEventHandler(int newDamageAmount, Transform displayLocation, Color damageColor);
        public event DamageDisplayEventHandler DamageDisplayEvent;
        

        public void DisplayDamage(int damageTaken)
        {
            if(DamageDisplayEvent != null)
            {
                DamageDisplayEvent(damageTaken, damageDisplayTransform, damageColorTint);
            }
        }

    }
}