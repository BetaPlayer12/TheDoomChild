using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class LandFX : PlayerFXCaller
    {
        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            //if (m_sensor.CompareTag("SolidPlatform"))
            //{
                var fx = GameplaySystem.fXManager.InstantiateFX<SpineFX>(m_fxPrefab, m_spawnPosition.position);
                fx.SetFacing(m_facing.currentFacingDirection);
           // }
        }

        private void Awake()
        {
            GetComponentInParent<Player>().GetComponentInChildren<ILandController>().LandCall += OnLandCall;
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