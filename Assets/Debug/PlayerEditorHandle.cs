#if UNITY_EDITOR
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Cinematics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class PlayerEditorHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_player;
        [SerializeField]
        private GameObject m_character;
        [SerializeField]
        private GameObject m_characterBehaviour;
        [SerializeField]
        private Transform m_characterCenterMass;
        [SerializeField]
        private AnimationParametersData m_animationParametersData;
        [SerializeField]
        private GameObject m_gameplaySystem;

        private static GameObject m_playerInstance;
        private static GameObject m_characterInstance;

        private void Awake()
        {
            if (m_playerInstance)
            {
                Destroy(m_player);
            }
            else
            {
                m_playerInstance = m_player;
            }
            if (m_characterInstance)
            {
                Destroy(m_character);
            }
            else
            {
                m_characterInstance = m_character;
            }
        }

        private void OnDestroy()
        {
            if (m_playerInstance == m_player)
            {
                m_playerInstance = null;
            }
            if (m_characterInstance == m_character)
            {
                m_characterInstance = null;
            }
        }

        [Button]
        private void ExecuteTransfer()
        {
            m_player.GetComponentInChildren<Player>().Initialize(m_character);
            m_player.GetComponentInChildren<StatToModelInjector>().Initialize(m_character);
            m_player.GetComponentInChildren<PlayerCharacterInitializer>().Initialize(m_character, m_animationParametersData, m_characterBehaviour);
            m_player.GetComponentInChildren<BestiaryProgressTracker>().Initialize(m_character);
            //m_player.GetComponentInChildren<PlayerCharacterControllerOld>().Initialize(m_character, m_characterBehaviour);
            if (m_gameplaySystem)
            {
                m_gameplaySystem.GetComponentInChildren<Cinema>().Initialize(m_characterCenterMass);
            }
        }
    }
}
#endif
