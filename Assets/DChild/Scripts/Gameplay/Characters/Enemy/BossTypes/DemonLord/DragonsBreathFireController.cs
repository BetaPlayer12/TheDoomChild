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
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using DChild.Temp;

public class DragonsBreathFireController : MonoBehaviour
{
    [SerializeField, TabGroup("FX animator")]
    private Animator m_dragonsBreathAnimatorFxSide1;
    [SerializeField, TabGroup("FX animator")]
    private Animator m_dragonsBreathAnimatorFxSide2;
    [SerializeField, TabGroup("FX animator")]
    private Animator m_dragonsBreathAnimator;
    [SerializeField, TabGroup("Values")]
    private float m_fireTrailDuration;
    [SerializeField, TabGroup("Values")]
    private float m_fadeDelayDuration;

    public void StartDragonsRoutine()
    {
        StartCoroutine(DragonsBreathFireTrailRoutine());
    }
    public void SetActiveDragonTrail(bool activate)
    {
       
            if (activate)
            {
                m_dragonsBreathAnimatorFxSide1.gameObject.SetActive(true);
                m_dragonsBreathAnimatorFxSide2.gameObject.SetActive(true);
            }
            else
            {
                m_dragonsBreathAnimatorFxSide1.gameObject.SetActive(false);
                m_dragonsBreathAnimatorFxSide2.gameObject.SetActive(false);
            }
        
       
    }
    public void StartDragonBreathRoutine(Spine.AnimationState animation, string dragonsAttackRight, string dragonsAttackLeft)
    {
        StartCoroutine(DragonsBreathControllerRoutine(animation, dragonsAttackRight, dragonsAttackLeft));
    }
    private IEnumerator DragonsBreathFireTrailRoutine()
    {
        yield return new WaitForSeconds(m_fireTrailDuration);
        m_dragonsBreathAnimatorFxSide1.SetTrigger("StartFade");
        m_dragonsBreathAnimatorFxSide2.SetTrigger("StartFade");
        yield return new WaitForSeconds(m_fadeDelayDuration);
        SetActiveDragonTrail(false);
        m_dragonsBreathAnimator.gameObject.SetActive(false);
    }
    private IEnumerator DragonsBreathControllerRoutine(Spine.AnimationState animation, string dragonsAttackRight, string dragonsAttackLeft)
    {
        m_dragonsBreathAnimator.SetTrigger("FireBreatheSide2");
        m_dragonsBreathAnimatorFxSide2.SetTrigger("FireTrailSide2");
        yield return new WaitForAnimationComplete(animation, dragonsAttackRight);
        m_dragonsBreathAnimator.SetTrigger("FireBreatheSide1");
        m_dragonsBreathAnimatorFxSide1.SetTrigger("FireTrailSide1");
        yield return new WaitForAnimationComplete(animation, dragonsAttackLeft);
        m_dragonsBreathAnimator.gameObject.SetActive(false);
        yield return null;
    }

}
