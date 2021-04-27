using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleInfiniteShadow : MonoBehaviour
    {

        [Button]
        public void ToggleOn()
        {

            GameplaySystem.playerManager.player.modifiers.Set(DChild.Gameplay.Characters.Players.PlayerModifier.ShadowMagic_Requirement, 0);
        }

        [Button]
        public void ToggleOff()
        {
            GameplaySystem.playerManager.player.modifiers.Set(DChild.Gameplay.Characters.Players.PlayerModifier.ShadowMagic_Requirement, 1);

        }
    }
}
