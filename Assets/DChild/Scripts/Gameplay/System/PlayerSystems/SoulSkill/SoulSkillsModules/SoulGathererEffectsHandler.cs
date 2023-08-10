using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
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

        var lootPicker = FindObjectOfType<LootPicker>();
        lootPicker.OnLootPickup += OnLootPickUp;
        lootPicker.OnLootPickupEnd += OnLootPickUpEnd;
    }

    private void OnLootPickUpEnd(object sender, EventActionArgs eventArgs)
    {
        m_effects.Stop();
    }

    private void OnLootPickUp(object sender, EventActionArgs eventArgs)
    {
        m_effects.Play();
    }
}
