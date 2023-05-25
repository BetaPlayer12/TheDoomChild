using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaofEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject m_toCreate;
    // Start is called before the first frame update
    void Start()
    {
        m_toCreate = Object.Instantiate(m_toCreate);
        m_toCreate.transform.localPosition = transform.position;
    }

    
}
