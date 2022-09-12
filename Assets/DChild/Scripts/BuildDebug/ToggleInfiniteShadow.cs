using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleInfiniteShadow : MonoBehaviour, IToggleDebugBehaviour
    {
        private bool m_isActive;
        private float m_originalValue;
        public bool value => m_isActive;

        [Button]
        public void ToggleOn()
        {
            m_isActive = true;
            GameplaySystem.playerManager.player.modifiers.Set(PlayerModifier.ShadowMagic_Requirement, 0);
        }

        [Button]
        public void ToggleOff()
        {
            m_isActive = false;
            GameplaySystem.playerManager.player.modifiers.Set(PlayerModifier.ShadowMagic_Requirement, 1);
        }

        private void Awake()
        {
            GameplaySystem.playerManager.player.modifiers.ModifierChange += OnChange;
        }

        private void Start()
        {
            m_originalValue = GameplaySystem.playerManager.player.modifiers.Get(PlayerModifier.ShadowMagic_Requirement);
        }

        private void OnChange(object sender, ModifierChangeEventArgs eventArgs)
        {
            if (eventArgs.modifier == PlayerModifier.ShadowMagic_Requirement)
            {
                m_originalValue = eventArgs.value;
                if (m_isActive)
                {
                    GameplaySystem.playerManager.player.modifiers.Set(PlayerModifier.ShadowMagic_Requirement, 0);
                }
            }
        }
    }
}
