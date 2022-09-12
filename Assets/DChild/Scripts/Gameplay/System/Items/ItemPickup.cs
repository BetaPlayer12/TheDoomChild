using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Doozy.Engine;
using Holysoft.Event;
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
        private Vector3 m_promptOffset;
        [SerializeField]
        private ItemData m_data;
        [SerializeField]
        private bool m_HasNotification;
        [SerializeField,ShowIf("m_HasNotification")]
        private string m_notifEvent;

        private bool m_hasBeenPickedUp;

        private Collider2D m_trigger;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public string promptMessage => "Pick up";

        public void Interact(Character character)
        {
            character.GetComponent<PlayerControlledObject>().owner.inventory.AddItem(m_data);
            m_model.SetActive(false);
            m_trigger.enabled = false;
            if (m_HasNotification == true)
            {
                //GameEventMessage.SendEvent("Soul Skill Acquired");
                GameEventMessage.SendEvent(m_notifEvent);
            }
            m_hasBeenPickedUp = true;
        }

        public ISaveData Save() => new SaveData(m_hasBeenPickedUp);

        public void Load(ISaveData data)
        {
            m_hasBeenPickedUp = ((SaveData)data).isPickedUp == false;
            m_model.SetActive(!m_hasBeenPickedUp);
            m_trigger.enabled = !m_hasBeenPickedUp;
        }
        public void Initialize()
        {
            m_model.SetActive(true);
            m_trigger.enabled = true;
        }
        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
        private void Awake()
        {
            m_trigger = GetComponentInChildren<Collider2D>();
        }
    }
}