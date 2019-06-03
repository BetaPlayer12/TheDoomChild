using DChild.Gameplay.Characters.Players.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public abstract class PlayerFXCaller : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        protected GameObject m_fxPrefab;
        [SerializeField]
        protected Transform m_spawnPosition;
        protected IFacing m_facing;

        public virtual void Initialize(IPlayerModules player)
        {
            m_facing = player;
        }
    }
}