using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Cinematics
{
    public class CameraOffsetEnvironmentConfigurator : MonoBehaviour
    {
        [SerializeField]
        private DChild.Gameplay.Cinematics.CameraPeekConfiguration m_config;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
            {
                GameplaySystem.cinema.SetCameraPeekConfiguration(m_config);

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
            {
                GameplaySystem.cinema.SetCameraPeekConfiguration(DChild.Gameplay.Cinematics.CameraPeekConfiguration.None);

            }
        }
    }
}
