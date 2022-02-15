using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using System;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    public class PlayerCombatHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_hitFX;
        [SerializeField]
        private PlayerIFrameHandle m_iFrameHandle;
        [SerializeField]
        private ReactivePlayerCamera m_reactiveCamera;
        [SerializeField]
        private PlayerHitStopHandle m_hitStopHandle;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void ResolveDamageRecieved(IPlayer player)
        {
            if (player.state?.canFlinch ?? true)
            {
                m_spawnHandle.InstantiateFX(m_hitFX, player.character.centerMass.position);
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
            m_spawnHandle = new FXSpawnHandle<FX>();
        }

    }
}