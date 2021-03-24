/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class LootChest : SerializedMonoBehaviour, IButtonToInteract, ISerializableComponent
    {
        [System.Serializable]
        private struct SaveData : ISaveData
        {
            [SerializeField]
            private bool m_isLooted;

            public SaveData(bool isLooted)
            {
                m_isLooted = isLooted;
            }

            public bool isLooted => m_isLooted;

            public ISaveData ProduceCopy() => new SaveData(m_isLooted);
        }

        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private ILootDataContainer m_loot;
        private bool m_isLooted;

        public bool showPrompt => m_isLooted == false;

        public string promptMessage => "Open";

        public Vector3 promptPosition => transform.position + m_promptOffset;
        public void Load(ISaveData data)
        {
           m_isLooted = ((SaveData)data).isLooted;
            if (m_isLooted)
            {
                //Force Player Animation?
                //Enable Cinematic Thingy?
                //Temporary Fix, If All Chest are the same dont make UnityEvent
                gameObject.SetActive(false);
                
            }
            else
            {
                //Temporary Fix, If All Chest are the same dont make UnityEvent
                gameObject.SetActive(true);
            }
        }

        public ISaveData Save() => new SaveData(m_isLooted);

        public void Interact(Character character)
        {
            m_loot.DropLoot(transform.position);
            m_isLooted = true;
            Debug.Log("Chest Opened");
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}