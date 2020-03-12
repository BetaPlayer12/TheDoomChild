using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLayerHandle : MonoBehaviour
{
    [SerializeField, OnValueChanged("UpdateSortingOrder")]
    private int m_referenceOrder;

#if UNITY_EDITOR

    private void UpdateSortingOrder()
    {
        var parentObjectRenderer = GetComponent<Renderer>();

        if(parentObjectRenderer != null)
        {
            parentObjectRenderer.sortingOrder += m_referenceOrder;

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                if (child != null)
                {
                    var childRenderer = child.GetComponent<Renderer>();

                    if (childRenderer != null)
                    {
                        childRenderer.sortingOrder += m_referenceOrder;
                    }
                }
            }
        }
    }

    [Button("Reset")]
    private void ResetOrderLayer()
    {
         
    }
#endif
}
