using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
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

        public bool showPrompt => true;

        public Vector3 promptPosition => m_promptPostion.position;

        public void Interact(Character character)
        {
            character.GetComponent<PlayerControlledObject>().owner.inventory.AddItem(m_data);
            gameObject.SetActive(false);
        }

        public ISaveData Save() => new SaveData(!gameObject.activeSelf);

        public void Load(ISaveData data) => gameObject.SetActive(((SaveData)data).isPickedUp == false);
    }
}