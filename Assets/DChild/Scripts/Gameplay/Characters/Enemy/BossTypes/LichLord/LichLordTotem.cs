using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Systems.WorldComponents;
using UnityEngine;

public class LichLordTotem : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_totemAttackFX;
    [SerializeField, TabGroup("FX")]
    private ParticleFX m_totemGroundFX;
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
    private string m_emergeAnimation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_emergeAttackAnimation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_emergeLoopAnimation;

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Animation")]
    private string m_submergeAnimation;

    private void Start()
    {
        m_spine.SetEmptyAnimation(0, 0);
        EmergeTotem();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            collision.GetComponentInParent<IsolatedObject>().Slower(.75f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponentInParent<IsolatedObject>().Slower(0);
    }

    public void EmergeTotem()
    {
        StartCoroutine(EmergeRoutine());
    }

    public void SubmergeTotem()
    {
        StartCoroutine(SubmergeRoutine());
    }

    private IEnumerator EmergeRoutine()
    {
        Debug.Log("EmERGE TOTEM");
        m_spine.SetAnimation(0, m_emergeAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_emergeAnimation);
        m_spine.SetAnimation(0, m_emergeAttackAnimation, false);
        m_totemGroundFX.Play();
        yield return new WaitForAnimationComplete(m_spine.animationState, m_emergeAttackAnimation);
        m_totemAttackFX.Play();
        Debug.Log("PLAY TOTEM FX");
        yield return null;
    }

    private IEnumerator SubmergeRoutine()
    {
        m_spine.SetAnimation(0, m_submergeAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_submergeAnimation);
        Destroy(this.gameObject);
        yield return null;
    }
}
