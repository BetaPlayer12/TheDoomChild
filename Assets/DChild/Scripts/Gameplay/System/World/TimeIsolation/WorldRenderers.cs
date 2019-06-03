using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    public class WorldRenderers : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private RendererObjects m_rendererObjects;

        private void OnEnable()
        {
            if (m_rendererObjects != null)
            {
                GameplaySystem.world.Register(m_rendererObjects);
            }
        }

        private void OnDisable()
        {
            if (m_rendererObjects != null)
            {
                GameplaySystem.world.Unregister(m_rendererObjects);
            }
        }

        private void OnValidate()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            if (renderers == null)
            {
                m_rendererObjects = null;
            }
            else
            {
                List<Material> materialList = new List<Material>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    materialList.Add(renderers[i].sharedMaterial);
                }

                m_rendererObjects = new RendererObjects(materialList.ToArray());
            }
        }
    }
}