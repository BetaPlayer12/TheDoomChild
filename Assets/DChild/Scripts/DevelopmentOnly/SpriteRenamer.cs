using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenamer : MonoBehaviour
{
    [SerializeField]
    private Transform m_scope;
    [SerializeField]
    private Sprite m_filter;
    [SerializeField]
    private string m_renameTo;
    [SerializeField, PropertyOrder(100)]
    private List<SpriteRenderer> m_sprites;

    [Button]
    private void Gather()
    {
        if (m_scope == null)
        {
            FilterSprites(FindObjectsOfType<SpriteRenderer>());
        }
        else
        {
            FilterSprites(m_scope.GetComponentsInChildren<SpriteRenderer>());
        }
    }

    private void FilterSprites(SpriteRenderer[] renderer)
    {
        m_sprites.Clear();
        for (int i = 0; i < renderer.Length; i++)
        {
            var render = renderer[i];
            if (render.sprite == m_filter)
            {
                m_sprites.Add(render);
            }
        }
    }

    [Button, ShowIf("@m_renameTo != null")]
    private void Rename()
    {
        for (int i = 0; i < m_sprites.Count; i++)
        {
            m_sprites[i].gameObject.name = $"{m_renameTo} ({i + 1})";
        }
    }
}
