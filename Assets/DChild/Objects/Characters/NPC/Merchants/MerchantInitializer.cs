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
}