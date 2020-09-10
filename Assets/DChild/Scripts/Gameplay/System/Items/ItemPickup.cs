using DChild.Gameplay.Characters.Players;
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

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isPickedUp);
        }
        [SerializeField]
        private GameObject m_model;
        [SerializeField]
        private Transform m_promptPosition;
        [SerializeField]
        private ItemData m_data;
        [SerializeField]
        private bool m_HasNotification;

        private Collider2D m_trigger;

        public bool showPrompt => true;

        public Vector3 promptPosition => m_promptPosition.position;

        public string promptMessage => "Pick up";

        public void Interact(Character character)
        {
            character.GetComponent<PlayerControlledObject>().owner.inventory.AddItem(m_data);
            m_model.SetActive(false);
            m_trigger.enabled = false;
            if (m_HasNotification == true)
            {
                GameEventMessage.SendEvent("Soul Skill Acquired");
            }
        }

        public ISaveData Save() => new SaveData(!gameObject.activeSelf);

        public void Load(ISaveData data)
        {
            var hasNotBeenPickedUp = ((SaveData)data).isPickedUp == false;
            m_model.SetActive(hasNotBeenPickedUp);
            m_trigger.enabled = hasNotBeenPickedUp;
        }

        private void Awake()
        {
            m_trigger = GetComponentInChildren<Collider2D>();
        }
    }
}