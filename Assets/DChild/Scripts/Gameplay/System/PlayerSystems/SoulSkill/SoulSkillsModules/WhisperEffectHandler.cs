using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.SoulSkills;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WhisperEffectHandler : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem m_particleeffects;
    [SerializeField]
    private VisualEffect m_visualeffects;
    [SerializeField]
    private bool m_isparticle = false;
    private void Start()
    {
        if (m_isparticle == true)
        {
            m_particleeffects.Stop();
        }
        else
        {
            m_visualeffects.Stop();
        }

        FurryWhisperer.Onstatechange += statechange;
;
        
    }

    private void statechange(object sender, FurryWhisperer.StateChangeEvent eventArgs)
    {
        
      if (eventArgs.isactive == true)
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
      else
        {
            if (m_isparticle == true)
            {
                m_particleeffects.Stop();
            }
            else
            {
                m_visualeffects.Stop();
            }
        }
    }
}
