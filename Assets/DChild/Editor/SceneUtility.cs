using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class SceneUtility
{
    private static List<Transform> m_transformList = new List<Transform>();
    private static List<SpriteRenderer> m_spriteRendererList = new List<SpriteRenderer>();

    [MenuItem("Tools/DChild Utility/Optimize Sprite Renderers In Scene")]
    private static void OptimizeSpriteRenderersInScene()
    {
        var sprites = Object.FindObjectsOfType<SpriteRenderer>();
        m_spriteRendererList.Clear();
        for (int i = 0; i < sprites.Length; i++)
        {
            ReorientSpriteRenderer(sprites[i]);
        }
        for (int i = 0; i < m_spriteRendererList.Count; i++)
        {
            ReorientSpriteRenderer(m_spriteRendererList[i]);
        }
        m_spriteRendererList.Clear();
    }

    private static void ReorientSpriteRenderer(SpriteRenderer renderer)
    {
        var transform = renderer.transform;
        var scale = transform.localScale;
        bool hasChanges = false;
        if (scale.x < 0)
        {
            scale.x *= -1;
            renderer.flipX = !renderer.flipX;
            hasChanges = true;
        }
        if (scale.y < 0)
        {
            scale.y *= -1;
            renderer.flipY = !renderer.flipY;
            hasChanges = true;
        }
        if (hasChanges)
        {
            if (transform.childCount > 0)
            {
                m_spriteRendererList.AddRange(transform.GetComponentsInChildren<SpriteRenderer>(true));
                m_transformList.Clear();
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    m_transformList.Add(child);
                }
                for (int i = 0; i < m_transformList.Count; i++)
                {
                    m_transformList[i].parent = null;
                }
            }
        }
        transform.localScale = scale;
        if (hasChanges)
        {
            for (int i = 0; i < m_transformList.Count; i++)
            {
                m_transformList[i].parent = transform;
            }
            m_transformList.Clear();
        }
    }
}
