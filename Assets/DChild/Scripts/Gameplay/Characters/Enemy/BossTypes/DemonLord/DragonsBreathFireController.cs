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
    private Animator m_dragonsBreathAnimatorLeft;
    [SerializeField, TabGroup("FX animator")]
    private Animator m_dragonsBreathAnimatorRight;

    [SerializeField]
    private GameObject m_rightCollider;
    [SerializeField]
    private GameObject m_leftCollider;
    [SerializeField, TabGroup("Values")]
    private float m_fireTrailDuration;
    [SerializeField, TabGroup("Values")]
    private float m_fadeDelayDuration;

    [SerializeField]
    private bool m_leftFireBreath = true;
    [SerializeField]
    private bool m_isFireRoutineDone;
  
    public void StartDragonsRoutine()
    {
        StartCoroutine(DragonsBreathFireTrailRoutine());
    }
    public void SetActiveDragonTrail(bool activate)
    {
       
            if (activate == true)
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
    public void StartDragonBreathRoutine()
    {

        if (m_isFireRoutineDone == false)
        {
            m_isFireRoutineDone = true;
            StopCoroutine(DragonsBreathFireTrailRoutine());
            SetActiveDragonTrail(false);
        }
        else
        {
            StartCoroutine(DragonsBreathControllerRoutine());
        }
   
    }
    private IEnumerator DragonsBreathFireTrailRoutine()
    {
        m_isFireRoutineDone = false;
        yield return new WaitForSeconds(m_fireTrailDuration);
        TriggerFade();
        m_leftCollider.SetActive(false);
        m_rightCollider.SetActive(false);
        m_isFireRoutineDone = true;
    }
    public void TriggerFade()
    {
        m_dragonsBreathAnimatorFxSide1.SetTrigger("StartFade");
        m_dragonsBreathAnimatorFxSide2.SetTrigger("StartFade");
    }
    private IEnumerator DragonsBreathControllerRoutine()
    {
          StopAllCoroutines();
   
          if (m_leftFireBreath == true)
          {
            m_leftCollider.SetActive(true);
            m_dragonsBreathAnimatorLeft.SetTrigger("FireBreatheSide2");
            m_dragonsBreathAnimatorFxSide2.SetTrigger("FireTrailSide2");
            
            m_leftFireBreath = false;

          }
         else
        {
            m_rightCollider.SetActive(true);
            m_dragonsBreathAnimatorRight.SetTrigger("FireBreatheSide1");
            m_dragonsBreathAnimatorFxSide1.SetTrigger("FireTrailSide1"); 
            m_leftFireBreath = true;
         }
 
        yield return null;
    }

}
