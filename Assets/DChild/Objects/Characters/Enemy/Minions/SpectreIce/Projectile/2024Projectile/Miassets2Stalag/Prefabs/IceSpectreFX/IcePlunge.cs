using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.AI;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine;
using Spine.Unity;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;

public class IcePlunge : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField]
    private RaySensor m_groundSensor;
    [SerializeField]
    private Collider2D m_collider;
    [SerializeField]
    private Rigidbody2D m_rb;
    [SerializeField]
    private Attacker m_attacker;
    [SerializeField]
    private AttackData m_contactDamage;
    [SerializeField]
    private AttackData m_fallingDamage;
    public GameObject m_playerDamageable;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_break1Animation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_break2Animation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_destroyAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_fallLoopAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_spawnStartAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_onGroundAnimation;

    [SerializeField, TabGroup("FX")]
    private ParticleFX m_formationFX;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_smokeFX;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_explosionFX;


    private IEnumerator StartFall()
    {
        m_attacker.SetData(m_fallingDamage);
        yield return new WaitForSeconds(.5f);
        while (!m_groundSensor.allRaysDetecting)
        {
            m_spine.SetAnimation(0, m_fallLoopAnimation, true);
            yield return null;
        }
        yield return StartCoroutine(BreakCoroutine());
    }
    private bool m_isPlayerDamaged = false;
    private IEnumerator BreakCoroutine()
    {
        if (m_groundSensor.allRaysDetecting)
        {
            if (m_isPlayerDamaged)
            {
                m_attacker.SetData(m_contactDamage);
                m_rb.gravityScale = 0;
                m_spine.SetAnimation(0, m_break2Animation, false);
                yield return new WaitForSeconds(1f);
                m_explosionFX.Play();
                m_spine.SetAnimation(0, m_destroyAnimation, false);
                m_collider.enabled = false;
                yield return new WaitForAnimationComplete(m_spine.animationState, m_destroyAnimation);
                m_isPlayerDamaged = false;
            }
            else
            {
                m_attacker.SetData(m_contactDamage);
                m_rb.gravityScale = 0;
                m_smokeFX.Play();
                m_spine.SetAnimation(0, m_onGroundAnimation, true);
                yield return new WaitForSeconds(1f);
                m_spine.SetAnimation(0, m_break1Animation, false);
                yield return new WaitForSeconds(1f);
                m_spine.SetAnimation(0, m_break2Animation, false);
                yield return new WaitForSeconds(1f);
                m_explosionFX.Play();
                m_spine.SetAnimation(0, m_destroyAnimation, false);
                m_collider.enabled = false;
                yield return new WaitForAnimationComplete(m_spine.animationState, m_destroyAnimation);
            }
        }
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_playerDamageable.GetComponent<Damageable>().DamageTaken += M_playerDamageable_DamageTaken;
        m_formationFX.Play();
        m_spine.SetAnimation(0, m_spawnStartAnimation, false);
        StartCoroutine(StartFall());
    }

    private void M_playerDamageable_DamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
    {
        m_isPlayerDamaged = true;
        //throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
