using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild
{
    [DisallowMultipleComponent]
    public class CompositeSortingGroup : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        private class GroupInfo
        {
            [SerializeField, HorizontalGroup("Info"), ChildGameObjectsOnly,LabelWidth(100f)]
            private SortingGroup m_sortingGroup;
            [SerializeField, HorizontalGroup("Info")]
            private int m_relativeOrder;

            public void SetSorting(int layer, int order)
            {
                m_sortingGroup.sortingLayerID = layer;
                m_sortingGroup.sortingOrder = m_relativeOrder + order;
                EditorUtility.SetDirty(m_sortingGroup);
            }
        }

        [System.Serializable]
        private class IndividualInfo
        {
            [SerializeField, HorizontalGroup("Info"), ChildGameObjectsOnly, LabelWidth(100f)]
            private Renderer m_renderer;
            [SerializeField, HorizontalGroup("Info")]
            private int m_relativeOrder;

            public void SetSorting(int layer, int order)
            {
                m_renderer.sortingLayerID = layer;
                m_renderer.sortingOrder = m_relativeOrder + order;
                EditorUtility.SetDirty(m_renderer);
            }
        }

        [SerializeField, SortingLayer, OnValueChanged("OnSortingChange")]
        private int m_layer;
        [SerializeField, OnValueChanged("OnSortingChange")]
        private int m_order;
        [SerializeField, ListDrawerSettings(DraggableItems = false), OnValueChanged("OnSortingChange", true),TabGroup("Group")]
        private GroupInfo[] m_groupList;
        [SerializeField, ListDrawerSettings(DraggableItems = false), OnValueChanged("OnSortingChange", true), TabGroup("Individual")]
        private IndividualInfo[] m_individualList;

        private void OnSortingChange()
        {
            for (int i = 0; i < m_groupList.Length; i++)
            {
                m_groupList[i].SetSorting(m_layer, m_order);
            }

            for (int i = 0; i < m_individualList.Length; i++)
            {
                m_individualList[i].SetSorting(m_layer, m_order);
            }
        }

        private void OnValidate()
        {
            if (PrefabUtility.IsPartOfPrefabAsset(this) == false)
            {
                OnSortingChange();
            }
        }
#endif
    }

}