using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DoubleJumpFX : PlayerFXCaller
    {
        private void OnJumpCall(object sender, EventActionArgs eventArgs)
        {
            //if (m_sensor.CompareTag("SolidPlatform"))
            //{
            var fx = GameplaySystem.fXManager.InstantiateFX<ParticleFX>(m_fxPrefab, m_spawnPosition.position);
            // }
        }

        private void Awake()
        {
            GetComponentInParent<Player>().GetComponentInChildren<IDoubleJumpController>().DoubleJumpCall += OnJumpCall;
            m_spawnPosition = GetComponentInParent<Player>().transform;
        }

#if UNITY_EDITOR
        public void Initialize(GameObject fx)
        {
            m_fxPrefab = fx;
        }
#endif
    }
}