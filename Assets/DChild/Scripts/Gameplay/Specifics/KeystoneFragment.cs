using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using Doozy.Engine;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    public class KeystoneFragment : MonoBehaviour, IButtonToInteract, ISerializableComponent
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
        private SpriteRenderer m_sprite;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private string m_eventPrefix;
        [SerializeField, MinValue(0), MaxValue("@(m_fragmentDatas.Length - 1)"), OnValueChanged("UpdateSprite")]
        private int m_fragmentIndex;
        [SerializeField, OnValueChanged("UpdateSprite")]
        private ItemData[] m_fragmentDatas;

        private Collider2D m_trigger;

        public bool showPrompt => true;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public string promptMessage => "Pick up";

        public void Interact(Character character)
        {
            var inventory = character.GetComponent<PlayerControlledObject>().owner.inventory;
            inventory.AddItem(m_fragmentDatas[m_fragmentIndex]);
            m_model.SetActive(false);
            m_trigger.enabled = false;
            GameEventMessage.SendEvent($"{m_eventPrefix}");
        }

        public ISaveData Save() => new SaveData(!gameObject.activeSelf);

        public void Load(ISaveData data)
        {
            var hasNotBeenPickedUp = ((SaveData)data).isPickedUp == false;
            m_model.SetActive(hasNotBeenPickedUp);
            m_trigger.enabled = hasNotBeenPickedUp;
        }

        private void UpdateSprite()
        {
            m_sprite.sprite = m_fragmentDatas[m_fragmentIndex].icon;
        }

        private void Awake()
        {
            m_trigger = GetComponentInChildren<Collider2D>();
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}
