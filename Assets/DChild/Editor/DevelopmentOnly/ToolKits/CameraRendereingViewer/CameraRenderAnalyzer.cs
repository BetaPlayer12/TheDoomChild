using DChild.Gameplay;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChildEditor.Utility
{
    public class CameraRenderAnalyzer
    {
        [System.Serializable]
        public class Info
        {
            [SerializeField, ReadOnly, PreviewField(100f, Sirenix.OdinInspector.ObjectFieldAlignment.Center), TableColumnWidth(110, false)]
            private Sprite m_sprite;
            [SerializeField, DisplayAsString, TableColumnWidth(50, false)]
            private int m_count;

            public int count => m_count;

            public Info(Sprite sprite, int count)
            {
                m_sprite = sprite;
                m_count = count;
            }
        }

        private Camera m_camera;
        [SerializeField]
        private Dictionary<Sprite, int> m_spriteToCountPair;

        private Info[] m_info;
        [SerializeField]
        private List<string> m_sortingLayers;

        private List<Renderer> m_visibleRenderers;
        private Renderer[] m_renderers;
        private Vector3 m_cameraPosition;
        private float m_fieldOfView;
        private bool m_isOrthographic;
        private bool m_firstTimeCamera;

        public List<string> sortingLayers => m_sortingLayers;

        public CameraRenderAnalyzer(Camera camera)
        {
            m_camera = camera;
            m_firstTimeCamera = true;
            m_spriteToCountPair = new Dictionary<Sprite, int>();
            m_visibleRenderers = new List<Renderer>();
            m_sortingLayers = new List<string>();
            m_renderers = null;
        }

        public void SetCamera(Camera camera)
        {
            m_camera = camera;
            m_firstTimeCamera = true;
        }

        public void RecordRenderers()
        {
            m_renderers = Object.FindObjectsOfType<SpriteRenderer>();
        }

        public Info[] AnalyzeShownSprites()
        {
            if (m_camera != null)
            {
                if (m_firstTimeCamera || CameraHasChanges())
                {
                    m_visibleRenderers.Clear();
                    m_spriteToCountPair.Clear();
                    m_sortingLayers.Clear();
                    for (int i = 0; i < m_renderers.Length; i++)
                    {
                        var renderer = m_renderers[i];
                        if (renderer != null)
                        {
                            if (renderer.IsVisibleFrom(m_camera))
                            {
                                m_visibleRenderers.Add(renderer);
                                if (renderer.GetType() == typeof(SpriteRenderer))
                                {
                                    var spriteRenderer = (SpriteRenderer)renderer;
                                    var sprite = spriteRenderer.sprite;
                                    if (sprite != null)
                                    {
                                        if (m_spriteToCountPair.ContainsKey(sprite) == false)
                                        {
                                            m_spriteToCountPair.Add(sprite, 1);

                                        }
                                        else
                                        {
                                            m_spriteToCountPair[sprite] += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    m_info = new Info[m_spriteToCountPair.Count];
                    int index = 0;
                    foreach (var key in m_spriteToCountPair.Keys)
                    {
                        m_info[index] = new Info(key, m_spriteToCountPair[key]);
                        index++;
                    }
                    m_info = m_info.OrderByDescending(x => x.count).ToArray();
                    m_sortingLayers.Clear();
                    for (int i = 0; i < m_visibleRenderers.Count; i++)
                    {
                        var layer = m_visibleRenderers[i].sortingLayerName;
                        if (m_sortingLayers.Contains(layer) == false)
                        {
                            m_sortingLayers.Add(layer);
                        }
                    }
                    UpdateCameraProperties();
                }
            }
            else
            {
                m_spriteToCountPair.Clear();
                m_info = null;
                m_sortingLayers.Clear();
            }
            return m_info;
        }

        private bool CameraHasChanges()
        {
            var positionChange = m_camera.transform.position != m_cameraPosition;
            var viewTypeChange = m_camera.orthographic != m_isOrthographic;
            var fieldOfViewChange = m_fieldOfView == (m_camera.orthographic ? m_camera.fieldOfView : m_camera.orthographicSize);
            return positionChange || viewTypeChange || fieldOfViewChange;
        }

        private void UpdateCameraProperties()
        {
            m_cameraPosition = m_camera.transform.position;
            m_isOrthographic = m_camera.orthographic;
            m_fieldOfView = (m_camera.orthographic ? m_camera.fieldOfView : m_camera.orthographicSize);
        }
    }
}