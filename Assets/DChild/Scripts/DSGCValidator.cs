using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSGCValidator : MonoBehaviour
{
    private List<GameObject> m_soundObjects = new List<GameObject>();
    private void OnValidate()
    {
        var childrenComponents = GetComponentsInChildren<AudioSource>();
       for(int x = 0; x<childrenComponents.Length; x++)
        {
            if(childrenComponents[x].dopplerLevel != 0)
            {
                childrenComponents[x].dopplerLevel = 0;

                m_soundObjects.Add(childrenComponents[x].gameObject);
            }
        }
    }
}
