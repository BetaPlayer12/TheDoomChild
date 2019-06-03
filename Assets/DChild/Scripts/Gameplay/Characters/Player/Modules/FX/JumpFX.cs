using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class JumpFX : PlayerFXCaller
    {
        private void OnJumpCall(object sender, EventActionArgs eventArgs)
        {
            //if (m_sensor.CompareTag("SolidPlatform"))
            //{
            var fx = GameplaySystem.fXManager.InstantiateFX<SpineFX>(m_fxPrefab, m_spawnPosition.position);
            fx.SetFacing(m_facing.currentFacingDirection);
            // }
        }

        private void Awake()
        {
            GetComponentInParent<Player>().GetComponentInChildren<IJumpController>().JumpCall += OnJumpCall;
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