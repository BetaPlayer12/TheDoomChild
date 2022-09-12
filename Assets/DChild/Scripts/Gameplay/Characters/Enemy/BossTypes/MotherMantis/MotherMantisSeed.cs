using DChild;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherMantisSeed : MonoBehaviour
{
    [SerializeField]
    private GameObject m_thornVine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            var instance = Instantiate(m_thornVine, transform.position, Quaternion.identity);
        }
    }
}
