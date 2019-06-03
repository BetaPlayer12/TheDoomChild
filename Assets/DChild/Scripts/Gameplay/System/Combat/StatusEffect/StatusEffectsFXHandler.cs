using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class StatusEffectsFXHandler : StatusEffectComponent
    {
        [SerializeField]
        private GameObject m_fx;
        private FX m_instance;

        protected override void OnEffectEnd(object sender, StatusEffectEventArgs eventArgs)
        {
            GameSystem.poolManager.GetOrCreatePool<FXPool>().AddToPool(m_instance);
            m_instance = null;
        }

        protected override void OnEffectStart(object sender, StatusEffectEventArgs eventArgs)
        {
            m_instance = GameplaySystem.fXManager.InstantiateFX<FX>(m_fx, eventArgs.reciever.GetComponentInChildren<Transform>().position);
            m_instance.Play();
        }
    }
}