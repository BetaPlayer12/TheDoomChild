using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChildDebug
{
    public class PostFlipScaleCleanUp : MonoBehaviour
    {
        [SerializeField]
        private Transform m_reference;

        private List<Transform> m_allReferenceChildren;
        private List<Transform> m_transformList;

        [Button]
        private void Execute()
        {
#if UNITY_EDITOR
            m_allReferenceChildren = new List<Transform>(m_reference.GetComponentsInChildren<Transform>());
            for (int i = m_allReferenceChildren.Count - 1; i >= 0; i--)
            {
                var GO = m_allReferenceChildren[i].gameObject;
                if (PrefabUtility.IsPartOfAnyPrefab(GO) || GO.GetComponent<Collider2D>() != null)
                {
                    m_allReferenceChildren.Remove(GO.transform);
                }
                EditorUtility.DisplayProgressBar("Post Flip Clean Up", $"Clean Up References ({i + 1f - m_allReferenceChildren.Count}/{m_allReferenceChildren.Count})", (i + 1f - m_allReferenceChildren.Count) / m_allReferenceChildren.Count);
            }
            m_transformList = new List<Transform>();
            for (int i = 0; i < m_allReferenceChildren.Count; i++)
            {
                FlipTransform(m_allReferenceChildren[i]);
                EditorUtility.DisplayProgressBar("Post Flip Clean Up", $"Rescaling ({i + 1}/{m_allReferenceChildren.Count})", (i + 1f) / m_allReferenceChildren.Count);
            }
            m_allReferenceChildren.Clear();
            EditorUtility.ClearProgressBar();
#endif
        }

        private void FlipTransform(Transform reference)
        {
            var scale = reference.localScale;
            if (reference.lossyScale.x < 0)
            {
                if (reference.childCount > 0)
                {
                    for (int i = 0; i < reference.childCount; i++)
                    {
                        m_transformList.Add(reference.GetChild(i));
                    }
                    reference.DetachChildren();
                    scale.x *= -1;
                    reference.localScale = scale;
                    for (int i = 0; i < m_transformList.Count; i++)
                    {
                        m_transformList[i].SetParent(reference);
                    }
                    m_transformList.Clear();
                }
            }
        }
    }

}