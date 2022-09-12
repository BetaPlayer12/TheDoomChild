using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
#endif

namespace DChild
{

    [DisallowMultipleComponent]
    public class SortingHandle : MonoBehaviour
    {
        [SerializeField]
        private bool m_includeInBuild;
        [SerializeField, SortingLayer, OnValueChanged("UpdateSorting"), LabelText("Reference Layer")]
        private int m_referenceLayer;
        [SerializeField, OnValueChanged("UpdateSorting")]
        private int m_referenceOrder;


        [SerializeField, DisableInEditorMode]
        private Renderer[] m_renderers;
        [SerializeField, HideInInspector]
        private int[] m_baseOrders;

        public int sortingLayerID => m_referenceLayer;
        public int sortingOrder => m_referenceOrder;

        public void SetOrder(int layerID, int sortingOrder)
        {
            m_referenceLayer = layerID;
            m_referenceOrder = sortingOrder;
            if (Application.isPlaying)
            {
                for (int i = 0; i < m_renderers.Length; i++)
                {
                    m_renderers[i].sortingLayerID = layerID;
                    m_renderers[i].sortingOrder = m_baseOrders[i] + sortingOrder;
                }
            }
#if UNITY_EDITOR
            else
            {
                for (int i = 0; i < m_rendererList.Count; i++)
                {
                    m_rendererList[i].UpdateSorting(m_referenceLayer, m_referenceOrder);
                }
            } 
#endif
        }

#if UNITY_EDITOR
        [System.Serializable]
        public class Info
        {
            [SerializeField, DrawWithUnity, HorizontalGroup("Group"), ReadOnly, LabelWidth(80)]
            private Renderer m_renderer;
            [SerializeField, HorizontalGroup("Group"), ReadOnly]
            private int m_baseOrder;

            public Renderer renderer => m_renderer;
            public int baseOrder => m_baseOrder;

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

        [SerializeField]
        private bool m_allowEditorUpdate = true;
        [SerializeField, HideInInspector]
        private int m_previousReferenceOrder;
        [SerializeField, ListDrawerSettings(DraggableItems = false, HideAddButton = true, OnTitleBarGUI = "OnListGUI", HideRemoveButton = true)]
        private List<Info> m_rendererList = new List<Info>();

        private void OnListGUI()
        {
            if (SirenixEditorGUI.IconButton(EditorIcons.Download))
            {
                m_rendererList.Clear();
                PopulateList();
            }
        }

        private void PopulateList()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                m_rendererList.Add(new Info(renderers[i], m_referenceOrder));
                m_rendererList[i].UpdateSorting(m_referenceLayer, m_referenceOrder);
            }
        }

        private void UpdateSorting()
        {
            if (m_allowEditorUpdate)
            {
                for (int i = 0; i < m_rendererList.Count; i++)
                {
                    m_rendererList[i].UseCurrentAsBaseOrder(m_previousReferenceOrder);
                    m_rendererList[i].UpdateSorting(m_referenceLayer, m_referenceOrder);
                }
                m_previousReferenceOrder = m_referenceOrder;
                EditorUtility.SetDirty(gameObject);
            }
            else
            {
                m_previousReferenceOrder = m_referenceOrder;
            }
        }

        private void OnValidate()
        {
            if (m_rendererList.Count == 0)
            {
                m_previousReferenceOrder = 0;
                PopulateList();
            }
            if (m_includeInBuild)
            {
                m_renderers = m_rendererList.Select(x => x.renderer).ToArray();
                m_baseOrders = m_rendererList.Select(x => x.baseOrder).ToArray();
            }
            else
            {
                m_renderers = null;
                m_baseOrders = null;
            }
        }
#endif
    }

}