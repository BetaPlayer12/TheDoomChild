﻿using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems.Lore
{
    public class LoreNote : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private LoreData m_data;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public string promptMessage => "Pick Up";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public void Interact(Character character)
        {
            GameplaySystem.gamplayUIHandle.ShowNotification(UI.StoreNotificationType.Lore);
            gameObject.SetActive(false);
        }

        [Button]
        private void Pickup()
        {
            Interact(null);
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}