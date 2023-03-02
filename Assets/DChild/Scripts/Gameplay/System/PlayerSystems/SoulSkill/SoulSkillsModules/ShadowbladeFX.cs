using DChild.Gameplay.Characters;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowbladeFX : MonoBehaviour
{
    [SerializeField]
    private SkeletonAnimation m_animation;
    [SerializeField]
    private AnimationReferenceAsset m_shadowBladeMixAnimation;
    [SerializeField, Range(0, 100)]
    private int m_trackNumber;
    [SerializeField]
    private bool m_canShadowblade;
    public bool canShadowblade => m_canShadowblade;


    //void Start()
    //{
    //    if (m_canShadowblade)
    //        m_animation.state.SetAnimation(m_trackNumber, m_shadowBladeMixAnimation, true);
    //}

    private void Update()
    {
        if (m_canShadowblade)
            m_animation.state.SetAnimation(m_trackNumber, m_shadowBladeMixAnimation, true);
    }

    public void EnableShadowblade()
    {
        m_canShadowblade = true;
        //m_animation.state.SetAnimation(m_trackNumber, m_shadowBladeMixAnimation, true);
    }

    public void DisableShadowblade()
    {
        m_canShadowblade = false;
        m_animation.state.SetEmptyAnimation(m_trackNumber, 0);
    }
}
