using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class ShadowDamageHandler : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_collider;
        private ShadowMorph m_shadowmorph;
        private ShadowDash m_shadowdash;
        private ShadowSlide m_shadowslide;

        private void Awake()
        {
            GameObject player = GetComponentInParent<Character>().gameObject;

            m_shadowmorph = player.GetComponentInChildren<ShadowMorph>();
            m_shadowdash = player.GetComponentInChildren<ShadowDash>();
            m_shadowslide = player.GetComponentInChildren<ShadowSlide>();

            m_shadowmorph.ExecuteModule += ShadowDamageActivate;
            m_shadowmorph.End += ShadowDamageDeactivate;
            m_shadowdash.ExecuteModule += ShadowDamageActivate;
            m_shadowdash.End += ShadowDamageDeactivate;
            m_shadowslide.ExecuteModule += ShadowDamageActivate;
            m_shadowslide.End += ShadowDamageDeactivate;
        }

        private void ShadowDamageActivate(object sender, EventActionArgs eventArgs)
        {
            m_collider.enabled = true;
        }

        private void ShadowDamageDeactivate(object sender, EventActionArgs eventArgs)
        {
            m_collider.enabled = false;
        }
    }
}
