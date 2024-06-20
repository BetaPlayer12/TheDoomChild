using DChild.Gameplay.Environment.Interractables;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems.Lore
{
    public class LoreNote : MonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private LoreData m_data;
        [SerializeField]
        private bool m_isPickedUp;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public string promptMessage => "Pick Up";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        [System.Serializable]
        public struct SaveData : ISaveData
        {
            public SaveData(bool isPickedUp) : this()
            {
                this.m_isPickedUp = isPickedUp;
            }

            [SerializeField]
            private bool m_isPickedUp;

            public bool isPickedUp => m_isPickedUp;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_isPickedUp);
        }

        private void Start()
        {
            if (m_isPickedUp)
            {
                gameObject.SetActive(false);
            }
        }

        public void Interact(Character character)
        {
            GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(UI.StoreNotificationType.Lore);
            gameObject.SetActive(false);
            m_isPickedUp = true;
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

        public ISaveData Save() => new SaveData(m_isPickedUp);

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            m_isPickedUp = saveData.isPickedUp;

            if (m_isPickedUp)
            {
                gameObject.SetActive(false);
            }

        }
        public void Initialize()
        {
            
        }
    }
}