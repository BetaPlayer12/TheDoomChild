using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichLordSpike : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField, TabGroup("Reference")]
    private Collider2D m_bodyCollider;
    [SerializeField, TabGroup("Reference")]
    private SpineRootMotion m_spineRootMotion;
    [SerializeField, TabGroup("Reference")]
    private Collider2D m_hurtbox;

#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animation)
    {
        m_spine = spineRoot;
    }
#endif

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_emerge1Animation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_emerge2Animation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_submerge1Animation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_submerge2Animation;

    private string m_emergeAnimation;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.IsTouchingLayers(DChildUtility.GetEnvironmentMask()))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    public void EmergeSpike()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(EmergeRoutine());
    }

    private IEnumerator EmergeRoutine()
    {
        m_emergeAnimation = UnityEngine.Random.Range(0, 2) == 0 ? m_emerge1Animation : m_emerge2Animation;
        m_spine.SetAnimation(0, m_emergeAnimation, false);
        yield return new WaitForSeconds(.1f);
        GetComponentInChildren<SkeletonAnimation>().maskInteraction = SpriteMaskInteraction.None;
        yield return new WaitForAnimationComplete(m_spine.animationState, m_emergeAnimation);
        m_hurtbox.gameObject.SetActive(true);
        yield return null;
    }

    public void SubmergeSpike()
    {
        StartCoroutine(SubmergeRoutine());
    }

    private IEnumerator SubmergeRoutine()
    {
        var submergeAnimation = m_emergeAnimation == m_emerge1Animation ? m_submerge1Animation : m_submerge2Animation;
        m_spine.SetAnimation(0, submergeAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, submergeAnimation);
        Destroy(this.gameObject);
        yield return null;
    }
}
