using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;

public class WardingCrystal : MonoBehaviour
{
    [SerializeField, TabGroup("Reference")]
    private Damageable m_damageable;
    [SerializeField, TabGroup("Reference")]
    protected SpineRootAnimation m_animation;
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_flinchAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_damageState;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_heavilyDamageState;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deathAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deadState;

    private float Maxhealth;
    private bool m_isDead;
    // Start is called before the first frame update
    void Start()
    {
        m_damageable.DamageTaken += OnDamageTaken;
        m_damageable.Destroyed += Ondeath;

        Maxhealth = m_damageable.health.maxValue;
        m_animation.SetAnimation(0, m_idleAnimation, true);
    }

    private void Ondeath(object sender, EventActionArgs eventArgs)
    {
        Debug.Log("Something hit me, im dead");
        if (m_isDead)
        {
            return;
        }
        StartCoroutine(DeathRoutine());
        throw new NotImplementedException();
    }

    private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
    {
        if(m_isDead)
        {
            return;
        }
        StartCoroutine(DamageTakenRoutine());
        throw new NotImplementedException();
    }

    IEnumerator DamageTakenRoutine()
    {
        if (m_damageable.health.currentValue <= (Maxhealth * 0.65))
        {
            if (m_damageable.health.currentValue <= (Maxhealth * 0.25))
            {
                m_animation.SetAnimation(0, m_heavilyDamageState, true);
            }
            else
            {
                m_animation.SetAnimation(0, m_damageState, true);
            }
        }
        else
        {
            m_animation.SetAnimation(0, m_idleAnimation, true);
        }
        m_animation.AddAnimation(1, m_flinchAnimation, false, 0f);
        //m_animation.SetAnimation(0, m_flinchAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_flinchAnimation);
        Debug.Log("is Dead? "+m_damageable.health.isEmpty);
    }

    IEnumerator DeathRoutine()
    {
        m_isDead = true;
        m_animation.SetAnimation(0, m_deathAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_flinchAnimation);
        m_animation.SetAnimation(0, m_deadState,true);
    }
}
