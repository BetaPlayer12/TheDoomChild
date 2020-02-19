using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBSpearProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject m_spearFX;
    [SerializeField]
    private Transform m_parentTF;

    private static FXSpawnHandle<FX> m_spawnHandol;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            var fx = m_spawnHandol.InstantiateFX(m_spearFX, transform.position);

            fx.transform.rotation = transform.rotation;
            fx.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, -transform.rotation.y);
        }
    }
}
