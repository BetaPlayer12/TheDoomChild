﻿using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class ItemPickup : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isPickedUp;

            public SaveData(bool isPickedUp)
            {
                m_isPickedUp = isPickedUp;
            }

            public bool isPickedUp => m_isPickedUp;
        }

        [SerializeField]
        private Transform m_promptPostion;
        [SerializeField]
        private ItemData m_data;
        [SerializeField]
        private bool m_HasNotification;

        public bool showPrompt => true;

        public Vector3 promptPosition => m_promptPostion.position;

        public string promptMessage => "Pick up";

        public void Interact(Character character)
        {
            character.GetComponent<PlayerControlledObject>().owner.inventory.AddItem(m_data);
            gameObject.SetActive(false);
            if (m_HasNotification == true)
            {
                GameEventMessage.SendEvent("Soul Skill Acquired");
            }
        }

        public ISaveData Save() => new SaveData(!gameObject.activeSelf);

        public void Load(ISaveData data) => gameObject.SetActive(((SaveData)data).isPickedUp == false);
    }
}