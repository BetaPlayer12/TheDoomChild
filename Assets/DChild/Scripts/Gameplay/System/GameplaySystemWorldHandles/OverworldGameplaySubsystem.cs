using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class OverworldGameplaySubsystem : MonoBehaviour
    {

        #region Modules
        private static DChild.Gameplay.Systems.PlayerManager m_playerManager;
        public static IPlayerManager playerManager => m_playerManager;
        #endregion

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

        private void AssignModules()
        {

        }

        private void Awake()
        {
            AssignModules();
        }

        public static void LoadGame()
        {
            throw new NotImplementedException();
        }

        public static void PauseGame()
        {
            throw new NotImplementedException();
        }

        public static void ResumeGame()
        {
            throw new NotImplementedException();
        }
    }
}

