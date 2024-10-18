using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Environment;
using UnityEngine.Events;


public class MerchantInitializer : MonoBehaviour
{
    [SerializeField, TabGroup("Reference")]
    private SkeletonAnimation m_SkeletonAnimation;
    [SerializeField, TabGroup("Reference")]
    private SpriteRenderer m_cartPosition;
    [SerializeField, TabGroup("Actions")]
    private UnityEvent Default, Interact;
    [SerializeField, Spine.Unity.SpineAnimation, TabGroup("Animation")]
    private List<string> m_Interact;
    [SerializeField, Spine.Unity.SpineAnimation, TabGroup("Animation")]
    private List<string> m_Idle;
    [SerializeField, TabGroup("Appearance"),OnValueChanged("CartValueChanged")]
    private bool m_startWithCart;
    [SerializeField, TabGroup("Appearance"),OnValueChanged("CartValueChanged")]
    private Sprite m_CartSprite;
    [SerializeField, TabGroup("Appearance"),OnValueChanged("MerchantValueChanged")]
    private SkeletonDataAsset m_MerchantSpineAnimation;

    private void Start()
    {
        DefaultAction();
    }
    public void DefaultAction()
    {
        Default?.Invoke();
        m_SkeletonAnimation.AnimationName= ChooseIdleAnim();
    }

    public void InteractAction()
    {
        Interact?.Invoke();
        m_SkeletonAnimation.AnimationName = ChooseInteractAnim();
    }

    string ChooseIdleAnim()
    {
        if(m_Idle.Count>1)
        {
            int x = UnityEngine.Random.Range(0, m_Idle.Count);
            return m_Idle[x];
        }else
        {
            return m_Idle[0];
        }
    }

    string ChooseInteractAnim()
    {
        if (m_Interact.Count > 1)
        {
            int x = UnityEngine.Random.Range(0, m_Interact.Count);
            return m_Interact[x];
        }
        else
        {
            return m_Interact[0];
        }
    }

    void CartValueChanged()
    {
        m_cartPosition.gameObject.SetActive(m_startWithCart);
        m_cartPosition.sprite = m_CartSprite;
    }

    void MerchantValueChanged()
    {
        m_SkeletonAnimation.skeletonDataAsset = m_MerchantSpineAnimation;
        m_SkeletonAnimation.Initialize(true);
        m_SkeletonAnimation.AnimationName = m_Idle[0];
    }
}