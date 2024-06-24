using DChild;
using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KP_ProjectileImpact : MonoBehaviour
{
    [SerializeField]
    private GameObject m_spikeEnvironmentImpact;
    [SerializeField]
    private GameObject m_spikePlayerImpact;
    private Vector3 m_position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            m_position = transform.position;
            //StartCoroutine(OnEnvironmentImpact());
            StartCoroutine(OnPlayerImpact());
        }
        /*else
        {
            m_position = transform.position;
            StartCoroutine(OnPlayerImpact());
        }*/
    }

    // Update is called once per frame
    /*private IEnumerator OnEnvironmentImpact()
    {
        Instantiate(m_spikeEnvironmentImpact, m_position, Quaternion.identity);
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1f);
        Destroy(m_spikeEnvironmentImpact);
        yield return null;
    }*/
    private IEnumerator OnPlayerImpact()
    {
        Instantiate(m_spikePlayerImpact, m_position, Quaternion.identity);
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1f);
        Destroy(m_spikePlayerImpact);
        yield return null;
    }
}
