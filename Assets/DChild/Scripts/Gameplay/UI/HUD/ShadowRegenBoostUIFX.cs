using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{

    public class ShadowRegenBoostUIFX : ActivatableUIFX
    {
        [SerializeField]
        private ShadowGaugeRegen m_regen;
        [SerializeField]
        private Image m_fx;
        [SerializeField]
        private bool m_startAsEnabled;

        public override bool isFXEnabled => m_fx.enabled;

        public override void Disable()
        {
            HideFX();
            m_regen.RegenDelayed -= OnRegenEnd;
            m_regen.RegenEnd -= OnRegenEnd;
            m_regen.RegenStarted -= OnRegenDelayed;
        }

        public override void Enable()
        {
            m_regen.RegenDelayed += OnRegenEnd;
            m_regen.RegenEnd += OnRegenEnd;
            m_regen.RegenStarted += OnRegenDelayed;

            if (m_regen.IsRegenerating())
            {
                ShowFX();
            }
        }

        private void ShowFX()
        {
            m_fx.enabled = true;
        }

        private void HideFX()
        {
            m_fx.enabled = false;
        }

        private void OnRegenDelayed(object sender, EventActionArgs eventArgs)
        {
            HideFX();
        }

        private void OnRegenEnd(object sender, EventActionArgs eventArgs)
        {
            ShowFX();
        }

        private void Start()
        {
            if (m_startAsEnabled)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }
    }
}