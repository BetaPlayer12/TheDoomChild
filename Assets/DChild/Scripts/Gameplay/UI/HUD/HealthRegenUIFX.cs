using DChild.Gameplay.Characters.Players.SoulSkills;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class HealthRegenUIFX : ActivatableUIFX
    {
        [SerializeField]
        private Image m_fx;

        private PassiveRegeneration.Handle m_handle;

        public override bool isFXEnabled => m_fx.enabled;

        public void SetReference(PassiveRegeneration.Handle handle)
        {
            if (m_handle != null)
            {
                UnsubscribeToRegenEvents();
            }
            m_handle = handle;
            if (isFXEnabled)
            {
                SubscribeToRegenEvents();
            }
        }


        public override void Enable()
        {
            if (isFXEnabled == false)
            {
                SubscribeToRegenEvents();
                if (m_handle.isRegenerating)
                {
                    ShowFX();
                }
                else
                {
                    HideFX();
                }
            }
        }

        public override void Disable()
        {
            if (m_handle != null)
            {
                UnsubscribeToRegenEvents();
                HideFX();
            }
        }

        private void UnsubscribeToRegenEvents()
        {
            m_handle.RegenStart -= OnRegenStart;
            m_handle.RegenEnd -= OnRegenEnd;
        }

        private void SubscribeToRegenEvents()
        {
            m_handle.RegenStart -= OnRegenStart;
            m_handle.RegenEnd -= OnRegenEnd;
        }

        private void ShowFX()
        {
            m_fx.enabled = true;
        }

        private void HideFX()
        {
            m_fx.enabled = false;
        }


        private void OnRegenEnd(object sender, EventActionArgs eventArgs)
        {
            HideFX();
        }

        private void OnRegenStart(object sender, EventActionArgs eventArgs)
        {
            ShowFX();
        }

    }
}