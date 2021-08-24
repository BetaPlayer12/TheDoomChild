using DChild;
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
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            var fx = m_spawnHandol.InstantiateFX(m_spearFX, transform.position);
            Debug.Log("Spear Rotation: " + transform.parent.rotation.z);
            if (transform.parent.rotation.eulerAngles.z < 270)
            {
                fx.transform.localScale = new Vector3(-1, -1, 1);
            }
            //var mainFX = fx.GetComponent<ParticleSystem>().main;
            //mainFX.startRotation = transform.localScale.x > 0 ? (float)Mathf.PI * .4f : (float)Mathf.PI + ((float)Mathf.PI * .5f);
            fx.transform.rotation = transform.rotation /** Quaternion.Euler(transform.localScale.x > 0 ? Vector3.forward : Vector3.back)*/;
            fx.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, -transform.rotation.y);
        }
    }
}
