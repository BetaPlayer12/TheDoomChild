using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
namespace DChild.Gameplay.Items
{
    public class ItemLoot : Loot
    {
        [SerializeField, OnValueChanged("OnDataChange")]
        private ItemData m_data;

#if UNITY_EDITOR
        [SerializeField]
        private SpriteRenderer m_spriteRenderer;

        private void OnDataChange()
        {
            m_spriteRenderer.sprite = m_data.icon;
            gameObject.name = m_data.itemName.Replace(" ", string.Empty) + "Loot";
        }

        [Button, HideInPrefabInstances]
        private void CreateLootReference()
        {
            var lootReference = ScriptableObject.CreateInstance<LootReference>();
            lootReference.Initialize(gameObject);

            var prefabPath = AssetDatabase.GetAssetPath(gameObject);
            var directory = Directory.GetParent(prefabPath);
            var path = $"{directory}\\{gameObject.name.Replace("Loot", string.Empty)}LootReference.asset";
            if (AssetDatabase.LoadAssetAtPath<LootReference>(path) == null)
            {
                AssetDatabase.CreateAsset(lootReference, path);
                AssetDatabase.SaveAssets();
            }
        }
#endif

        public void SetData(ItemData data)
        {
            m_data = data;
#if UNITY_EDITOR
            OnDataChange();
#endif
        }

        public override void PickUp(IPlayer player)
        {
            m_pickedBy = player;
        }

        protected override void ApplyPickUp()
        {
            base.ApplyPickUp();
            if (m_pickedBy.inventory.HasSpaceFor(m_data))
            {
                m_pickedBy.inventory.AddItem(m_data);
            }
            else if (m_data is UsableItemData)
            {
                ((UsableItemData)m_data).Use(m_pickedBy);
            }
            DisableEnvironmentCollider();
        }
    }
}
