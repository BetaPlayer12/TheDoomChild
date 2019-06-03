using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DashFX : PlayerFXCaller
    {
        private void OnDashCall(object sender, EventActionArgs eventArgs)
        {
            var fx = GameplaySystem.fXManager.InstantiateFX<SpineFX>(m_fxPrefab, m_spawnPosition.position);
            fx.SetFacing(m_facing.currentFacingDirection);
        }

        private void Awake()
        {
            GetComponentInParent<IDashController>().DashCall += OnDashCall;
        }
    }
}