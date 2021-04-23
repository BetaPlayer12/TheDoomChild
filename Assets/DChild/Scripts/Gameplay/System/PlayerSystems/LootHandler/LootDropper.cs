using Holysoft.Event;
using DChild.Gameplay.Combat;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace DChild.Gameplay.Systems
{
    public class LootDropper : MonoBehaviour
    {
        [SerializeField]
        private LootData m_loot;
        [SerializeField]
        private bool m_dropWhenDestroyed;
        private Damageable m_damageable;

        [Button, HideInPrefabAssets]
        public void DropLoot()
        {
            if (m_loot != null)
            {
                m_loot.DropLoot(m_damageable.position);
            }
        }

        public void SetLootData(LootData lootData)
        {
            m_loot = lootData;
        }

        private void Awake()
        {
            m_damageable = GetComponentInParent<Damageable>();
            if (m_dropWhenDestroyed)
            {
                m_damageable.Destroyed += OnDestroyed;
            }
        }

        private void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            DropLoot();
        }

#if UNITY_EDITOR
        [Button, ShowIf("@m_loot == null"), HideInPrefabInstances]
        private void CreateLootData()
        {
            var lootReference = ScriptableObject.CreateInstance<LootData>();
            var prefabPath = AssetDatabase.GetAssetPath(gameObject);
            var directory = Directory.GetParent(prefabPath);
            var path = $"{directory}\\{gameObject.name}LootData.asset";
            var existingFile = AssetDatabase.LoadAssetAtPath<LootData>(path);
            if (existingFile == null)
            {
                AssetDatabase.CreateAsset(lootReference, path);
                m_loot = AssetDatabase.LoadAssetAtPath<LootData>(path);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
            else
            {
                m_loot = existingFile;
            }
            Selection.activeObject = m_loot;
        }
#endif
    }
}