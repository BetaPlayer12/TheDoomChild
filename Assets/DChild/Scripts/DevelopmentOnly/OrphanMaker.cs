using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrphanMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject m_childToBeOrphaned;

    public void CreateOrphan()
    {
        if(m_childToBeOrphaned.transform.parent != null)
        {
            m_childToBeOrphaned.transform.parent = null;
        }
    }
}
