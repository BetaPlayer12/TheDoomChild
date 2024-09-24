using DChild;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableDaggerImpactFX : MonoBehaviour
{
    [SerializeField, BoxGroup("Reference")]
    private float m_deathTimer;
    [SerializeField, BoxGroup("Reference")]
    private Projectile m_projectile;
    [SerializeField, BoxGroup("Reference")]
    private Rigidbody2D m_righidybody2D;
    [SerializeField, BoxGroup("Reference")]
    private Collider2D m_hurtbox;
    [SerializeField, BoxGroup("FX")]
    private Animator m_fxAnimator;
    [SerializeField, BoxGroup("FX")]
    private ParticleSystem m_impactFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject))
        {
            StartCoroutine(PoolRoutine());
        }
    }

    private IEnumerator PoolRoutine()
    {
        m_hurtbox.enabled = false;
        m_impactFX.Play();
        m_righidybody2D.velocity = Vector2.zero;
        int randomNumber = Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                m_fxAnimator.SetTrigger("impact");
                break;
            case 1:
                m_fxAnimator.SetTrigger("impact2");
                break;
            case 2:
                m_fxAnimator.SetTrigger("impact3");
                break;
        }
        yield return new WaitForSeconds(m_deathTimer);
        m_projectile.CallPoolRequest();
        yield return null;
    }

    private void OnEnable()
    {
        m_hurtbox.enabled = true;
    }
}
