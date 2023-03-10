using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class ShadowDamageHandler : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private ParticleSystem m_particleeffects;
        [SerializeField]
        private VisualEffect m_visualeffects;
        [SerializeField]
        private bool m_isparticle = false;
        private ShadowMorph m_shadowmorph;
        private ShadowDash m_shadowdash;
        private ShadowSlide m_shadowslide;
        private Attacker m_attacker;

        private void Start()
        {
            GameObject player = GetComponentInParent<Character>().gameObject;
            if (m_isparticle == true)
            {
                m_particleeffects.Stop();
            }
            else
            {
                m_visualeffects.Stop();
            }

            m_shadowmorph = player.GetComponentInChildren<ShadowMorph>();
            m_shadowdash = player.GetComponentInChildren<ShadowDash>();
            m_shadowslide = player.GetComponentInChildren<ShadowSlide>();
            m_attacker = this.GetComponent<Attacker>();

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
            m_attacker.TargetDamaged += ActivateEffect;

        }

        private void ActivateEffect(object sender, CombatConclusionEventArgs eventArgs)
        {
            if (m_isparticle == true)
            {
                m_particleeffects.Stop();
                m_particleeffects.Play();
            }
            else
            {
                m_visualeffects.Stop();
                m_visualeffects.Play();
            }
        }

        private void ShadowDamageDeactivate(object sender, EventActionArgs eventArgs)
        {
            m_collider.enabled = false;
            m_attacker.TargetDamaged -= ActivateEffect;
        }
        private void OnDestroy()
        {
            if (m_isparticle == true)
            {
                m_particleeffects.Stop();
            }
            else
            {
                m_visualeffects.Stop();
            }

            m_shadowmorph.ExecuteModule -= ShadowDamageActivate;
            m_shadowmorph.End -= ShadowDamageDeactivate;
            m_shadowdash.ExecuteModule -= ShadowDamageActivate;
            m_shadowdash.End -= ShadowDamageDeactivate;
            m_shadowslide.ExecuteModule -= ShadowDamageActivate;
            m_shadowslide.End -= ShadowDamageDeactivate;
        }
    }
}
