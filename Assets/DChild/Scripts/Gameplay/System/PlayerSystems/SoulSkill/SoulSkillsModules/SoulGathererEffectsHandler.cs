using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SoulGathererEffectsHandler : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem m_effects;

    private void Start()
    {
        m_effects.Stop();
        this.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
    }
    public void PlayEffect()
    {
        m_effects.Play();
    }
    public void StopEffect()
    {
        m_effects.Stop();
    }
    
}
