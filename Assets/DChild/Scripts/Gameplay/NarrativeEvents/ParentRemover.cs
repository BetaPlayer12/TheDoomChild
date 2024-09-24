using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentRemover : MonoBehaviour
{
    [SerializeField]
    private GameObject m_childWhoWillBeRemovedFromParent;

    public void RemoveChildsParent()
    {
        if(m_childWhoWillBeRemovedFromParent.transform.parent != null)
        {
            m_childWhoWillBeRemovedFromParent.transform.parent = null;
        }
    }
}
