using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using DChild.Gameplay.Items;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild
{
    public abstract class DatabaseAssetList<T> : SerializedScriptableObject where T : DatabaseAsset
    {
        [SerializeField, ReadOnly, HideReferenceObjectPicker]
        protected Dictionary<int, T> m_list = new Dictionary<int, T>();
        [SerializeField, ReadOnly]
        protected int[] m_IDs;

        public int Count => m_IDs.Length;
        public T GetInfo(int ID)
        {
            var item = m_list.ContainsKey(ID) ? m_list[ID] : null;
            if (item == null)
            {
#if UNITY_EDITOR
                var temp = new List<ItemData>(AssetDatabase.FindAssets("t:ItemData").Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid))));
                for (int x = 0; x < temp.Count; x++)
                {
                    if (temp[x].id == ID)
                    {
                        throw new UnityException("Missing from item list" + temp[x].itemName);
                    }
                }
#endif
            }
            return item;
        }

        public int[] GetIDs() => m_IDs;

#if UNITY_EDITOR

        [ShowInInspector, ListDrawerSettings(OnTitleBarGUI = "TitleBar", DraggableItems = true), AssetSelector]
        private List<T> m_hash = new List<T>();

        private void TitleBar()
        {
            if (m_hash.Count > 0)
            {
                if (SirenixEditorGUI.ToolbarButton(EditorIcons.Download))
                {
                    m_list.Clear();
                    m_IDs = new int[m_hash.Count];
                    for (int i = 0; i < m_hash.Count; i++)
                    {
                        var info = m_hash.ElementAt(i);
                        if (m_list.ContainsKey(info.id) == false)
                        {
                            m_list.Add(info.id, info);
                            m_IDs[i] = info.id;
                        }
                    }
                    CopyToList(m_hash);
                }
                if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
                {
                    m_hash.Clear();
                }
            }
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.File))
            {
                m_hash.Clear();
                foreach (var value in m_list.Values)
                {
                    m_hash.Add(value);
                }
            }
        }

        protected void CopyToList(IReadOnlyCollection<T> infos)
        {
            m_list.Clear();
            m_IDs = new int[infos.Count];
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos.ElementAt(i);
                if (m_list.ContainsKey(info.id) == false)
                {
                    m_list.Add(info.id, info);
                    m_IDs[i] = info.id;
                }
            }
        }
#endif
    }
}