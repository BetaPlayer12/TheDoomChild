using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild
{
    [DisallowMultipleComponent]
    public class SortingHandle : MonoBehaviour
    {
        [SerializeField, SortingLayer, OnValueChanged("UpdateSorting")]
        private int m_referenceLayer;
        [SerializeField, OnValueChanged("UpdateSorting")]
        private int m_referenceOrder;
        public int sortingLayerID => m_referenceLayer;
        public int sortingOrder => m_referenceOrder;

#if UNITY_EDITOR
        [System.Serializable]
        public class Info
        {
            [SerializeField, DrawWithUnity, HorizontalGroup("Group"), ReadOnly, LabelWidth(80)]
            private Renderer m_renderer;
            [SerializeField, HorizontalGroup("Group"), ReadOnly]
            private int m_baseOrder;

            public Info(Renderer renderer, int referenceOrder)
            {
                m_renderer = renderer;
                m_baseOrder = m_renderer.sortingOrder - referenceOrder;
            }

            public void UpdateSorting(int layer, int referenceOrder)
            {
                m_renderer.sortingLayerID = layer;
                m_renderer.sortingOrder = m_baseOrder + referenceOrder;
            }

            public void UseCurrentAsBaseOrder(int referenceOrder)
            {
                m_baseOrder = m_renderer.sortingOrder - referenceOrder;
            }
        }

        [SerializeField, HideInInspector]
        private int m_previousReferenceOrder;
        [SerializeField, ListDrawerSettings(DraggableItems = false, HideAddButton = true, OnTitleBarGUI = "OnListGUI", HideRemoveButton = true)]
        private List<Info> m_renderers;

        private void OnListGUI()
        {
            if (SirenixEditorGUI.IconButton(EditorIcons.Download))
            {
                m_renderers.Clear();
                PopulateList();
            }
        }

        private void PopulateList()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                m_renderers.Add(new Info(renderers[i], m_referenceOrder));
                m_renderers[i].UpdateSorting(m_referenceLayer, m_referenceOrder);
            }
        }

        private void UpdateSorting()
        {
            for (int i = 0; i < m_renderers.Count; i++)
            {
                m_renderers[i].UseCurrentAsBaseOrder(m_previousReferenceOrder);
                m_renderers[i].UpdateSorting(m_referenceLayer, m_referenceOrder);
            }
            m_previousReferenceOrder = m_referenceOrder;
        }

        private void OnValidate()
        {
            if (m_renderers.Count == 0)
            {
                m_previousReferenceOrder = 0;
                PopulateList();
            }
        }
#endif
    }

}