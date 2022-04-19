using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteToPrefabSwap : SerializedMonoBehaviour
{
    [SerializeField]
    private Transform m_scope;
    [SerializeField]
    private Dictionary<Sprite, GameObject> m_reference;

#if UNITY_EDITOR
    [Button]
    private void SwapAllSprites()
    {
        if (m_scope != null)
        {
            var renderer = m_scope.GetComponentsInChildren<SpriteRenderer>();
            Undo.RecordObjects(renderer, "Sprite Swap");
            SwapRenderers(renderer);
        }
        else
        {
            var renderer = FindObjectsOfType<SpriteRenderer>();
            Undo.RecordObjects(renderer, "Sprite Swap");
            SwapRenderers(renderer);
        }


    }

    private void SwapRenderers(SpriteRenderer[] renderers)
    {
        Dictionary<Sprite, int> indexing = new Dictionary<Sprite, int>();
        for (int i = renderers.Length - 1; i >= 0; i--)
        {
            var renderer = renderers[i];
            if (m_reference.ContainsKey(renderer.sprite) == false)
                continue;

            if (PrefabUtility.IsPartOfPrefabInstance(renderers[i]) == false)
            {
                var instantiatedObject = (GameObject)PrefabUtility.InstantiatePrefab(m_reference[renderer.sprite]);
                instantiatedObject.name = instantiatedObject.name + $" ({GenerateNewIndex(renderer.sprite)})";
                var instantiatedTransform = instantiatedObject.transform;
                instantiatedTransform.SetParent(renderer.transform.parent);
                instantiatedTransform.localPosition = renderer.transform.localPosition;
                instantiatedTransform.localRotation = renderer.transform.localRotation;
                instantiatedTransform.localScale = renderer.transform.localScale;

                var instantiatedRenderer = instantiatedObject.GetComponent<SpriteRenderer>();
                instantiatedRenderer.color = renderer.color;

                var sortingHandle = instantiatedObject.GetComponent<SortingHandle>();
                if (sortingHandle)
                {
                    sortingHandle.SetOrder(renderer.sortingLayerID, renderer.sortingOrder);
                }
                else
                {
                    instantiatedRenderer.sortingLayerID = renderer.sortingLayerID;
                    instantiatedRenderer.sortingOrder = renderer.sortingOrder;
                }
            }
            DestroyImmediate(renderer.gameObject);
        }

        int GenerateNewIndex(Sprite sprite)
        {
            if (indexing.ContainsKey(sprite))
            {
                indexing[sprite] += 1;
                return indexing[sprite];
            }
            else
            {
                indexing.Add(sprite, 1);
                return 1;
            }
        }
    }
#endif
}
