using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShrineVisualHandle : SkillShrineVisuals
{
    [SerializeField]
    private GameObject m_shrineObject;
    [SerializeField]
    List<GameObject> m_children = new List<GameObject>();
    [SerializeField]
    private Material m_defaultMaterial;


    public void GetChidren()
    {

        m_children.Clear();
        foreach (Transform transform in m_shrineObject.GetComponentInChildren<Transform>())
        {
            m_children.Add(transform.gameObject);

            foreach (Transform childtransform in transform)
            {
                m_children.Add(childtransform.gameObject);

            }
        }
    }

    [Button]
    public void SkillShrineState(bool shrineState)
    {
        GetChidren();
        if (shrineState)
        {
            for(int x = 0; x < m_children.Count; x++)
            {
                var renderer = m_children[x].GetComponent<SpriteRenderer>();
                if (renderer)
                {
                    renderer.material = m_defaultMaterial;
                }
                else
                {
                    m_children[x].gameObject.SetActive(false);
                }
            }
        }
    }
}
