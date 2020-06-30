using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlackTentacles : MonoBehaviour
{
    [SerializeField]
    private float m_duration;
    [SerializeField]
    private SpineRootAnimation m_spine;
    [SerializeField]
    private GameObject m_colliderDamage;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animationF, SkeletonAnimation animationB)
    {
        m_spine = spineRoot;
    }
#endif
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_startAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_idleAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
    private string m_endAnimation;

    private IEnumerator SpawnRoutine()
    {
        m_spine.SetAnimation(0, m_startAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_startAnimation);
        m_spine.SetAnimation(0, m_idleAnimation, true);
        yield return new WaitForSeconds(m_duration);
        m_colliderDamage.SetActive(false);
        m_spine.SetAnimation(0, m_endAnimation, false);
        yield return new WaitForAnimationComplete(m_spine.animationState, m_endAnimation);
        Destroy(this.gameObject);
        //Destroy or Pool here idk
        yield return null;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());

    }
}
