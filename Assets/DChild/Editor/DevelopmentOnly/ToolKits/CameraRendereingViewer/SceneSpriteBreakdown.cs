using DChild;
using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildEditor.Utility
{
    public class SceneSpriteBreakdown
    {


        [System.Serializable]
        public class SpriteBreakDown
        {
            [SerializeField, PreviewField(100f, ObjectFieldAlignment.Center)]
            private GameObject m_gameObject;
            [SerializeField, DisplayAsString, TableColumnWidth(100, false)]
            private string m_layer;

            public SpriteBreakDown(GameObject gameObject, string sortingLayer)
            {
                m_gameObject = gameObject;
                m_layer = sortingLayer;
            }
        }
        [SerializeField]
        private int m_sortLayer;

        [SerializeField]
        private Camera m_camera;

        private bool m_sortLayers;

        [SerializeField]
        private Dictionary<GameObject, string> m_ObjectToLayerPair;
        private SpriteBreakDown[] m_spriteBreakInfo;

        private List<Renderer> m_renderersList;
        private Renderer[] m_renderers;

        private Vector3 m_cameraPosition;
        private float m_fieldOfView;
        private bool m_isOrthographic;
        private bool m_isFirstCamera;


        public SceneSpriteBreakdown(Camera camera)
        {
            m_camera = camera;
            m_isFirstCamera = true;
            m_ObjectToLayerPair = new Dictionary<GameObject, string>();
            m_renderersList = new List<Renderer>();
            m_renderers = null;

        }

        public void SetSortingLayer(int layerName, bool useSortLayer)
        {
            m_sortLayers = useSortLayer;
            m_sortLayer = layerName;
        }

        public void GetSpriteRenderers()
        {
            m_renderers = Object.FindObjectsOfType<SpriteRenderer>();
        }

        public void SetCamera(Camera camera)
        {
            m_camera = camera;
            m_isFirstCamera = true;
        }

        public SpriteBreakDown[] AnalyzedObjectRenderers()
        {
            if(m_camera != null)
            {
                if (m_isFirstCamera || CameraChanges())
                {
                    m_ObjectToLayerPair.Clear();
                    m_renderersList.Clear();
                    for (int i = 0; i < m_renderers.Length; i++)
                    {
                        var currentRenderer = m_renderers[i];

                        if (currentRenderer != null)
                        {
                            if (currentRenderer.IsVisibleFrom(m_camera))
                            {
                                if(currentRenderer.GetType() == typeof(SpriteRenderer))
                                {
                                    var spriteRenderer = (SpriteRenderer)currentRenderer;
                                    var sprite = spriteRenderer.sprite;
                                    if(sprite != null)
                                    {
                                        if(m_ObjectToLayerPair.ContainsKey(spriteRenderer.gameObject) == false)
                                        {
                                            if (m_sortLayers)
                                            {
                                                if (spriteRenderer.sortingLayerID == m_sortLayer)
                                                {
                                                    m_ObjectToLayerPair.Add(spriteRenderer.gameObject, spriteRenderer.sortingLayerName);

                                                }
                                            }
                                            else
                                            {
                                                m_ObjectToLayerPair.Add(spriteRenderer.gameObject, spriteRenderer.sortingLayerName);
                                            }
                                           

                                        }
                                        else
                                        {
                                            m_ObjectToLayerPair[spriteRenderer.gameObject] += 1;
                                        }
                                    }
                                }

                            }
                        }
                    }

                    m_spriteBreakInfo = new SpriteBreakDown[m_ObjectToLayerPair.Count];
                    int index = 0;
                    foreach(var key in m_ObjectToLayerPair.Keys)
                    {
                        m_spriteBreakInfo[index] = new SpriteBreakDown(key, m_ObjectToLayerPair[key]);
                        index++;
                    }
                    UpdateCamera();
                }
              
            }
            else
            {
                m_ObjectToLayerPair.Clear();
                m_spriteBreakInfo = null;
            }
            return m_spriteBreakInfo;
        }

        private bool CameraChanges()
        {
            var positionChange = m_camera.transform.position != m_cameraPosition;
            var viewTypeChange = m_camera.orthographic != m_isOrthographic;
            var fieldOfviewChange = m_fieldOfView != (m_camera.orthographic ? m_camera.fieldOfView : m_camera.orthographicSize);
            return positionChange || viewTypeChange || fieldOfviewChange;
        }

        private void UpdateCamera()
        {
            m_cameraPosition = m_camera.transform.position;
            m_isOrthographic = m_camera.orthographic;
            m_fieldOfView = (m_camera.orthographic ? m_camera.fieldOfView : m_camera.orthographicSize);
        }
    }
}

