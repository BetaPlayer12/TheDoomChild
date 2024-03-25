/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DarkTonic.MasterAudio;
using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using DChild.Gameplay.UI;
using DChild.Serialization;
using Holysoft.Event;
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
        private LootChestVisual m_visuals;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private ILootDataContainer m_loot;
        private bool m_isLooted;

        private static LootList m_lootList;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

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
                m_visuals.Open(true);
                GetComponent<Collider2D>().enabled = false;
                //gameObject.SetActive(false);

            }
            else
            {
                //Temporary Fix, If All Chest are the same dont make UnityEvent
                m_visuals.Close(true);
                GetComponent<Collider2D>().enabled = true;
                //gameObject.SetActive(true);
            }
        }
        public void Initialize()
        {
            m_isLooted = false;
            gameObject.SetActive(true);
        }
        public ISaveData Save() => new SaveData(m_isLooted);

        [Button]
        public void Interact(Character character)
        {
            m_isLooted = true;
            if (m_loot != null)
            {
                GivePlayerLoot();
                SendNotification();
            }

            InteractionOptionChange?.Invoke(this, EventActionArgs.Empty);
            ShowOpenChestVisual();
        }

        private void ShowOpenChestVisual()
        {
            m_visuals.Open();
            GetComponent<Collider2D>().enabled = false;
            GetComponent<EventSounds>().ActivateCodeTriggeredEvent1();
            GetComponent<VFXSpawner>().Spawn();
        }

        private void GivePlayerLoot()
        {
            var playerInventory = GameplaySystem.playerManager.player.inventory;
            m_lootList.Clear();
            m_loot.GenerateLootInfo(ref m_lootList);
            var lootItems = m_lootList.GetAllItems();
            for (int i = 0; i < lootItems.Length; i++)
            {
                var item = lootItems[i];
                playerInventory.AddItem(item, m_lootList.GetCountOf(item));
            }
            if (m_lootList.soulEssenceAmount > 0)
            {
                float souls = m_lootList.soulEssenceAmount * GameplaySystem.modifiers.SoulessenceAbsorption;
                playerInventory.AddSoulEssence((int)souls);
            }
        }

        private void SendNotification()
        {
            //Notify UI of loot chest content
            GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(m_lootList);
        }

        private void Awake()
        {
            if (m_lootList == null)
            {
                m_lootList = new LootList();
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}