#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class SpriteHighlightImplementor : MonoBehaviour
    {
        [SerializeField]
        private Material m_material;
        [SerializeField]
        private Color m_color = Color.white;
        [SerializeField]
        private Transform m_scope;
        [SerializeField]
        private List<Sprite> m_acceptableSprites;


        [Button, HideIf("@m_scope == null")]
        private void ImplementHighlight()
        {
            var renderers = m_scope.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (m_acceptableSprites.Count > 1)
                {
                    if (m_acceptableSprites.Contains(renderer.sprite) == false)
                    {
                        continue;
                    }
                }
                renderer.sharedMaterial = m_material;
                renderer.color = m_color;
            }
        }
    }
}
#endif