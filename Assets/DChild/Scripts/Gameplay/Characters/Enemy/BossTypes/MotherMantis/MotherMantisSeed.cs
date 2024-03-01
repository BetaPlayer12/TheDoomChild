using DChild;
using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherMantisSeed : MonoBehaviour
{
    [SerializeField]
    private GameObject m_thornVine;
    [SerializeField]
    private GameObject m_bladeOfGrass;
    [SerializeField]
    private Vector3 m_position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            m_position = transform.position;
            StartCoroutine(Growing());
        }
    }
    private IEnumerator Growing()
    {
        Instantiate(m_bladeOfGrass, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(m_thornVine, m_position, Quaternion.identity);
        yield return null;
    }
}
