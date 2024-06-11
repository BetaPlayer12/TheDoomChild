using DChild;
using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CB_ShotgunBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bulletImpact;
    private Vector3 m_position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            m_position = transform.position;
            StartCoroutine(OnImpact());
        }
    }

    // Update is called once per frame
    private IEnumerator OnImpact()
    {
        Instantiate(m_bulletImpact, m_position, Quaternion.identity);
        Destroy(this.gameObject);
        yield return new WaitForSeconds(1f);
        Destroy(m_bulletImpact);
        yield return null;
    }
}
