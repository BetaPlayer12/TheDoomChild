using DChild.Gameplay.Characters;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField]
    private float m_hitStopTime;
    [SerializeField]
    private float m_whiteDecayTime;
    [SerializeField]
    private bool m_enableHitStop;
    [SerializeField, TabGroup("Reference")]
    protected SpineRootAnimation m_animation;
    [SerializeField, TabGroup("Modules")]
    private FlinchHandler m_flinchHandle;
    [SerializeField, TabGroup("Renderer")]
    private MeshRenderer m_Rendererer;

    private float m_highlightCurrentValue;
    //private IEnumerator m_flinchWhiteRoutine;

    private void OnHitStopStart(object sender, EventActionArgs eventArgs)
    {
        Debug.Log("HitStop");
        StopAllCoroutines();
        StartCoroutine(FlinchWhiteRoutine());
        if (m_enableHitStop)
        {
            StartCoroutine(HitStopRoutine());
        }
    }

    private void StartRoutines()
    {
        StopAllCoroutines();
        StartCoroutine(FlinchWhiteRoutine());
        if (m_enableHitStop)
        {
            StartCoroutine(HitStopRoutine());
        }
    }

    private IEnumerator HitStopRoutine()
    {
        m_animation.animationState.TimeScale = 0;
        var currentTrackTime = m_animation.animationState.GetCurrent(0).TrackTime;
        yield return new WaitForSeconds(m_hitStopTime);
        m_animation.animationState.GetCurrent(0).TrackTime = currentTrackTime + m_hitStopTime;
        m_animation.animationState.TimeScale = 1;
        yield return null;
    }

    private IEnumerator FlinchWhiteRoutine()
    {
        m_Rendererer.material.SetFloat("Highlight", 1);
        while (m_Rendererer.material.GetFloat("Highlight") >= .1f)
        {
            m_highlightCurrentValue -= Time.deltaTime * m_whiteDecayTime;
            m_Rendererer.material.SetFloat("Highlight", m_highlightCurrentValue);
            yield return null;
        }
        m_Rendererer.material.SetFloat("Highlight", 0);
        m_highlightCurrentValue = 1;
        yield return null;
    }

    private void Awake()
    {
        m_flinchHandle.HitStopStart += OnHitStopStart;
        m_highlightCurrentValue = 1;
        //m_flinchWhiteRoutine = FlinchWhiteRoutine();
    }
}
