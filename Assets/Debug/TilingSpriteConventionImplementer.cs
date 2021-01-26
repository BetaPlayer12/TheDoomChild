using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChildDebug
{

    public class TilingSpriteConventionImplementer : MonoBehaviour
    {
        [SerializeField]
        private Sprite m_tilingSprite;
        [SerializeField]
        private GameObject m_fromReference;

        [Button]
        private void ImplementConvention()
        {
            var spriteRenderers = m_fromReference.GetComponentsInChildren<SpriteRenderer>();
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                var spriteRenderer = spriteRenderers[i];
                if (spriteRenderer.sprite == m_tilingSprite)
                {
                    children.Clear();
                    var spriteTransform = spriteRenderer.transform;
                    for (int j = 0; j < spriteTransform.childCount; j++)
                    {
                        children.Add(spriteTransform.GetChild(j));
                    }
                    spriteTransform.DetachChildren();

                    spriteRenderer.sortingLayerName = "PlayableGround";
                    if (spriteRenderer.drawMode == SpriteDrawMode.Simple)
                    {
                        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
                        var size = spriteRenderer.size;
                        var lossyScale = spriteTransform.lossyScale;
                        size.x = 5 * lossyScale.x;
                        size.y = 5 * lossyScale.y;
                        spriteRenderer.size = size;
                        spriteTransform.localScale = Vector3.one;
                    }

                    for (int j = 0; j < children.Count; j++)
                    {
                        children[j].parent = spriteTransform;
                    }
                    if (spriteRenderer.TryGetComponent(out BoxCollider2D collider))
                    {
                        collider.autoTiling = true;
                    }
                }

            }
        }
    }

}