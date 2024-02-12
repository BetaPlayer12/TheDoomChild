using DChild;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalStalagtite : MonoBehaviour
{
    //[SerializeField]
    //private struct Info
    //{
    //    //Animations
    //    [SerializeField, ValueDropdown("GetAnimations")]
    //    private string m_growAnimation;
    //    public string growAnimation => m_growAnimation;
    //    [SerializeField, ValueDropdown("GetAnimations")]
    //    private string m_idleAnimation;
    //    public string idleAnimation => m_idleAnimation;
    //    [SerializeField, ValueDropdown("GetAnimations")]
    //    private string m_deathAnimation;
    //    public string deathAnimation => m_deathAnimation;
    //    [SerializeField, ValueDropdown("GetAnimations")]
    //    private string m_death2Animation;
    //    public string death2Animation => m_death2Animation;
    //    [SerializeField, ValueDropdown("GetAnimations")]
    //    private string m_flinchAnimation;
    //    public string flinchAnimation => m_flinchAnimation;
    //    [SerializeField, ValueDropdown("GetAnimations")]
    //    private string m_flinch2Animation;
    //    public string flinch2Animation => m_flinch2Animation;
    //}

    //[SerializeField]
    //private Info m_info;
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_growthAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_flinchAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_flinchAnimation2;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deathAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_deathAnimation2;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_wiltAnimation;

    [SerializeField]
    private SpineRootAnimation m_animation;
    [SerializeField]
    private GameObject m_colliders;
    [SerializeField]
    private Damageable m_damageable;

    private IEnumerator GrowthRoutine()
    {
        m_animation.SetAnimation(0, m_growthAnimation, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_growthAnimation);
        m_animation.SetAnimation(0, m_idleAnimation, true);
        m_colliders.SetActive(true);
        yield return null;
    }

    private IEnumerator DeathFxRoutine()
    {
        m_animation.SetAnimation(0, m_deathAnimation, false);
        yield return null;

    }

    private IEnumerator WiltFxRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        m_animation.SetAnimation(0, m_wiltAnimation, false);
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void Start()
    {
        StartCoroutine(GrowthRoutine());
    }

    private void LateUpdate()
    {

    }

}
