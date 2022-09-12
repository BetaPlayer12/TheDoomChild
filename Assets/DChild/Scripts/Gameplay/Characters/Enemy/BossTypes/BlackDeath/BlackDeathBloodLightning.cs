using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BlackDeathBloodLightning : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_hurtBoxCollider;
    [SerializeField]
    private float m_timeToWait;
    [SerializeField]
    private float m_timeToActivateHurtbox;
    [SerializeField]
    private float m_timeToDeactivateHurtbox;
    [SerializeField, TabGroup("FX")]
    private ParticleSystem m_indicatorFX;
    [SerializeField, TabGroup("FX")]
    private ParticleSystem m_lightningFX;

    private IEnumerator HurtboxRoutine()
    {
        m_indicatorFX.Play();
        yield return new WaitForSeconds(m_timeToWait);
        m_lightningFX.Play();
        yield return new WaitForSeconds(m_timeToActivateHurtbox);
        m_hurtBoxCollider.enabled = true;
        yield return new WaitForSeconds(m_timeToDeactivateHurtbox);
        m_hurtBoxCollider.enabled = false;
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
        yield return null;
    }

    private void Awake()
    {
        m_hurtBoxCollider.enabled = false;
        StartCoroutine(HurtboxRoutine());
    }
}
