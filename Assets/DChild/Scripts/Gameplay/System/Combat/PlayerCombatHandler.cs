﻿using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    public class PlayerCombatHandler : MonoBehaviour
    {
        [SerializeField]
        private PlayerIFrameHandle m_iFrameHandle;
        [SerializeField]
        private ReactivePlayerCamera m_reactiveCamera;
        [SerializeField]
        private PlayerHitStopHandle m_hitStopHandle;

        private HitStopHandle m_hitStop;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void ResolveDamageRecieved(IPlayer player)
        {
            if (player.state?.canFlinch ?? true)
            {
                m_hitStop.Execute();
                m_reactiveCamera.HandleOnDamageRecieveShake();
                m_hitStopHandle.Execute(false);
                StartCoroutine(m_iFrameHandle.DisableInputTemporarily(player));
                StartCoroutine(m_iFrameHandle.ExecuteTemporaryInvulnerability(player));
            }
        }

        public void ResolveDamageDealt(CombatConclusionEventArgs eventArgs)
        {
            m_reactiveCamera.HandleOnAttackHit(eventArgs);
            m_hitStopHandle.Execute(true);
        }

        private void Awake()
        {
            m_hitStop = GetComponent<HitStopHandle>();
        }
    }
}