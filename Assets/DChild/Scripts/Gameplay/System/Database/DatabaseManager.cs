using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using DChildEditor;
using System.Linq;
#endif


namespace DChild.Gameplay.Databases
{
    public interface IDatabaseManager
    {
        T GetDatabase<T>() where T : ScriptableDatabase;
    }

    public class DatabaseManager : SerializedMonoBehaviour, IGameplaySystemModule, IDatabaseManager
    {
        [SerializeField, PropertyOrder(2), ReadOnly]
        private Dictionary<Type, ScriptableDatabase> m_databases = new Dictionary<Type, ScriptableDatabase>();

        public T GetDatabase<T>() where T : ScriptableDatabase
        {
            var databaseType = typeof(T);
            return m_databases.ContainsKey(databaseType) ? (T)m_databases[databaseType] : null;
        }

#if UNITY_EDITOR
        [NonSerialized, OdinSerialize, HideReferenceObjectPicker, PropertyOrder(1), HideInPrefabInstances, ValueDropdown("GetAllAssets", IsUniqueList = true), LabelText("Database")]
        private List<ScriptableDatabase> list = new List<ScriptableDatabase>();

        [Button, PropertyOrder(0), HideInPrefabInstances]
        private void Validate()
        {
            if (m_databases == null)
            {
                m_databases = new Dictionary<Type, ScriptableDatabase>();
            }

            m_databases?.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                var table = list[i];
                if (m_databases.ContainsKey(table.GetType()) == false)
                {
                    m_databases.Add(table.GetType(), table);
                }
            }
        }

        private static IEnumerable GetAllAssets()
        {
            var root = DChildResources.ScriptableObjectFolder + "/";
            return UnityEditor.AssetDatabase.GetAllAssetPaths()
                .Where(x => x.StartsWith(root))
                .Select(x => x.Substring(root.Length))
                .Select(x => new ValueDropdownItem<ScriptableDatabase>(x.Split('.')[0], UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableDatabase>(root + x)))
                .Where(x => x.Value != null);
        }
#endif
    }
}