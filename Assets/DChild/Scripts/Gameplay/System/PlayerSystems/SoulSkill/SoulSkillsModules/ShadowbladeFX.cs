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

    
    //void Start()
    //{
    //    m_animation.state.SetAnimation(m_trackNumber, m_shadowBladeMixAnimation, false);
    //}

    public void EnableShadowblade()
    {
        m_animation.state.SetAnimation(m_trackNumber, m_shadowBladeMixAnimation, false);
    }

    public void DisableShadowblade()
    {
        m_animation.state.SetEmptyAnimation(m_trackNumber, 0);
    }
}
