using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPusCutsceneAnimationMix : MonoBehaviour
{
    [SerializeField]
    private SpineRootAnimation m_spine;
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    public void InitializeField(SpineRootAnimation spineRoot, SkeletonAnimation animation)
    {
        m_spine = spineRoot;
        m_skeletonAnimation = animation;
        //m_skeletonBAnimation = animationB;
    }
#endif

    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Front")]
    private string m_phase1MixAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Front")]
    private string m_phase2MixAnimation;
    [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), TabGroup("Front")]
    private string m_phase3MixAnimation;
    [SerializeField]
    private Phase m_currentPhase;

    private enum Phase
    {
        Phase1,
        Phase2,
        Phase3,
    }

    private void Start()
    {
        switch (m_currentPhase)
        {
            case Phase.Phase1:
                m_spine.SetAnimation(11, m_phase1MixAnimation, false);
                break;
            case Phase.Phase2:
                m_spine.SetAnimation(11, m_phase2MixAnimation, false);
                break;
            case Phase.Phase3:
                m_spine.SetAnimation(11, m_phase3MixAnimation, false);
                break;
        }
    }
}
