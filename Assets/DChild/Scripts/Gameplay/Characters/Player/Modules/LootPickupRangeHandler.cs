using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LootPickupRangeHandler : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private CircleCollider2D m_collider;
        private IPlayerModifer m_modifier;
        private float value;
        public void Initialize(ComplexCharacterInfo info)
        {
            m_modifier = info.modifier;
        }
            private void Start()
        {
           
            m_modifier.ModifierChange += ExtendRange;
        }
            private void ExtendRange(object sender, ModifierChangeEventArgs eventArgs)
        {
            if (eventArgs.modifier== PlayerModifier.SoulAbsorb_Range)
            {
               
                if (eventArgs.value == 1)
                {
                    m_collider.radius -= value;
                }
                else
                {
                    value = eventArgs.value;
                    m_collider.radius += value;
                }

            }

        }
 
    }
}
