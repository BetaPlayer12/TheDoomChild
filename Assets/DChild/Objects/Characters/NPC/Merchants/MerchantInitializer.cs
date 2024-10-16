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
    private string m_Interact;
    [SerializeField, Spine.Unity.SpineAnimation, TabGroup("Animation")]
    private string m_Idle;
    [SerializeField, TabGroup("Initialize")]
    private bool m_startWithCart;
    [SerializeField, TabGroup("Initialize")]
    private Sprite m_CartSprite;

    private void Start()
    {
        DefaultAction();
        if(!m_startWithCart)
        {
            m_cartPosition.gameObject.SetActive(false);
            return;
        }
        m_cartPosition.sprite = m_CartSprite;
    }
    public void DefaultAction()
    {
        Default?.Invoke();
        m_SkeletonAnimation.AnimationName= m_Idle;
    }

    public void InteractAction()
    {
        Interact?.Invoke();
        m_SkeletonAnimation.AnimationName = m_Interact;
    }
}