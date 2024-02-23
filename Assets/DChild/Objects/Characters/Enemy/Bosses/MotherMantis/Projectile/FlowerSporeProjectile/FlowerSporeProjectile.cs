using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSporeProjectile : MonoBehaviour
{
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_growthAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_idle;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_waitForInput;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_sporeAttack1;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_sporeAttack2;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_sporeAttack3;

    [SerializeField, TabGroup("References")]
    private SkeletonAnimation m_skeletonAnimation;
    [SerializeField, TabGroup("References")]
    private SpineRootAnimation m_animation;

    [SerializeField, TabGroup("FX")]
    private ParticleFX m_bulbGrowthAnitciaption;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_bulbAnitciaption;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_bulbExplosion;


    private IEnumerator GrowthRoutine()
    {
        m_bulbGrowthAnitciaption.Play();
        yield return new WaitForSeconds(1f);
        m_animation.SetAnimation(0, m_growthAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_growthAnimation);
        m_animation.SetAnimation(0, m_idle, true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(BulbExplosionRoutine());
        yield return null;
    }

    private IEnumerator BulbExplosionRoutine()
    {
        m_bulbAnitciaption.Play();
        m_animation.SetAnimation(0, m_sporeAttack1, false);
        yield return new WaitForSeconds(1f);
        m_bulbExplosion.Play();
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        StartCoroutine(GrowthRoutine());
    }


}
