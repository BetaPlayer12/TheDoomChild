using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StoneBuiltEffectsHandler : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem m_effects;
    private void Start()
    {
        GameplaySystem.playerManager.player.health.ValueChanged += OnStatChange;
        this.transform.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
    }

    private void OnStatChange(object sender, StatInfoEventArgs eventArgs)
    {
        m_effects.Stop();
        m_effects.Play();
    }
}
