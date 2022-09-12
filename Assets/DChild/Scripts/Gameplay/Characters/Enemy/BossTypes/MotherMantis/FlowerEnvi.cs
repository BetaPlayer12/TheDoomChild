using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnvi : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField]
    private bool m_autoFlinch;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonFAnimation;
    [SerializeField]
    private SkeletonAnimation m_skeletonBAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animationF, SkeletonAnimation animationB)
    {
        m_spine = spineRoot;
        //m_skeletonFAnimation = animationF;
        //m_skeletonBAnimation = animationB;
    }
#endif
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonFAnimation"), TabGroup("Front")]
    private string m_closeFIdleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonFAnimation"), TabGroup("Front")]
    private string m_openFIdleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonFAnimation"), TabGroup("Front")]
    private string m_closeFAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonFAnimation"), TabGroup("Front")]
    private string m_openFAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonBAnimation"), TabGroup("Back")]
    private string m_closeBIdleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonBAnimation"), TabGroup("Back")]
    private string m_openBIdleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonBAnimation"), TabGroup("Back")]
    private string m_closeBAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonBAnimation"), TabGroup("Back")]
    private string m_openBAnimation;

    private IEnumerator OpenFRoutine()
    {
        m_spine.SetAnimation(0, m_openFAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_openFAnimation);
        m_spine.SetAnimation(0, m_openFIdleAnimation, true);
        yield return null;
    }

    private IEnumerator OpenBRoutine()
    {
        m_spine.SetAnimation(0, m_openBAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_openBAnimation);
        m_spine.SetAnimation(0, m_openBIdleAnimation, true);
        yield return null;
    }

    private IEnumerator CloseFRoutine()
    {
        m_spine.SetAnimation(0, m_closeFAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_closeFAnimation);
        m_spine.SetAnimation(0, m_closeFIdleAnimation, true);
        yield return null;
    }

    private IEnumerator CloseBRoutine()
    {
        m_spine.SetAnimation(0, m_closeBAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_closeBAnimation);
        m_spine.SetAnimation(0, m_closeBIdleAnimation, true);
        yield return null;
    }

    public void OpenFlower()
    {
        //StartCoroutine(OpenFRoutine());
        //StartCoroutine(OpenBRoutine());
    }

    public void CloseFlower()
    {
        //StartCoroutine(CloseFRoutine());
        //StartCoroutine(CloseBRoutine());
    }

    private void Start()
    {
        m_spine.SetAnimation(0, m_closeFIdleAnimation, true);
        m_spine.SetAnimation(0, m_closeBIdleAnimation, true);
    }
}
