using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class OverworldGameplaySubsystem : MonoBehaviour
    {

        #region Modules

        #endregion

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

        private void AssignModules()
        {

        }

        private void Awake()
        {
            AssignModules();
        }
    }
}

