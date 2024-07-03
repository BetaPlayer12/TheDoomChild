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

public class SpectreIceShield : MonoBehaviour
{
    public GameObject m_spectreIceAI;
    public ParticleFX m_shieldHolder;

    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField]
    private BasicHealth m_health;
    [SerializeField]
    private Collider2D m_hitbox;
    [SerializeField]
    private Hitbox m_spectreHitbox;
    [SerializeField]
    private Damageable m_damageable;

    private int m_damageCount;
    private int m_damageReceived;

    public EventAction<EventActionArgs> OnShieldDestroy;
    public EventAction<EventActionArgs> OnActivate;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_activateAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_destroyedAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_inactiveAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_loopingIdleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_only1ShieldLeftAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_only1LeftShardBreakAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_only2ShieldLeftAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_only2LeftShardBreakAnimation;

    public void InitializeField(SpineRootAnimation spineRoot)
    {
        m_spine = spineRoot;
    }
    protected void Awake()
    {
        m_spectreIceAI.GetComponent<SpectreIceAI>().OnDetection += OnDetection;
        m_damageable.DamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
    {
        //throw new NotImplementedException();
        m_damageReceived = m_health.maxValue - m_health.currentValue;
        if (m_damageReceived >= 90)
        {
            m_damageCount = 3;
        }
        else if (m_damageReceived >= 60)
        {
            m_damageCount = 2;
        }
        else if (m_damageReceived >= 30)
        {
            m_damageCount = 1;
        }
        StartCoroutine(ShieldCount(m_damageCount));
    }

    private void OnDetection(object sender, EventActionArgs eventArgs)
    {
        OnActivate?.Invoke(this, EventActionArgs.Empty);
        m_shieldHolder.Play();
        m_spine.SetAnimation(0, m_activateAnimation, false);
        m_spine.SetAnimation(0, m_loopingIdleAnimation, true);
        m_hitbox.enabled = true;
        //throw new NotImplementedException();
    }
    public void Restart()
    {
        m_damageable.health.SetHealthPercentage(1f);
        OnActivate?.Invoke(this, EventActionArgs.Empty);
        m_hitbox.enabled = true;
        m_damageCount = 0;
        m_shieldHolder.Play();
        m_spine.SetAnimation(0, m_activateAnimation, false);
        m_spine.SetAnimation(0, m_loopingIdleAnimation, true);
        m_hitbox.enabled = true;
    }
    private IEnumerator ShieldCount(int damageCount)
    {
        switch (damageCount)
        {
            case 1:
                m_spine.SetAnimation(0, m_only2LeftShardBreakAnimation, false);
                yield return new WaitForAnimationComplete(m_spine.animationState, m_only2LeftShardBreakAnimation);
                m_spine.SetAnimation(0, m_only2ShieldLeftAnimation, true);
                break;
            case 2:
                m_spine.SetAnimation(0, m_only1LeftShardBreakAnimation, false);
                yield return new WaitForAnimationComplete(m_spine.animationState, m_only1LeftShardBreakAnimation);
                m_spine.SetAnimation(0, m_only1ShieldLeftAnimation, true);
                break;
            case 3:
                OnShieldDestroy?.Invoke(this, EventActionArgs.Empty);
                m_shieldHolder.Stop();
                m_spine.SetAnimation(0, m_destroyedAnimation, false);
                yield return new WaitForAnimationComplete(m_spine.animationState, m_destroyedAnimation);
                m_spine.SetAnimation(0, m_inactiveAnimation, false);
                m_hitbox.enabled = false;
                break;
        }
        yield return null;
    }
    // Start is called before the first frame update
    private void Start()
    {
        int maxHealth = 90;
        m_damageable.health.SetMaxValue(maxHealth);
        m_damageable.health.SetHealthPercentage(1f);
    }

    // Update is called once per frame
    private void Update()
    {/*
        Debug.Log(m_health.currentValue);*/
    }

}
