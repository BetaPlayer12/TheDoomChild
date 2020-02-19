using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDroneAudio : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonDataAsset m_asset;
#endif

    [SerializeField]
    private AudioClip m_audioClip1;
    [SerializeField]
    private AudioClip m_audioClip2;

    [SerializeField]
    private AudioSource m_audioSource;



    Spine.EventData eventData;
    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<SkeletonAnimation>();


        animation.AnimationState.Event += HandleAnimationStateEvent;
    }

    // Update is called once per frame
    private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {

        bool eventMatch = (eventData == e.Data);
        if (eventMatch)
        {
            RangeEvt();
            DeadEvt();
        }
    }

    public void RangeEvt()
    {
        m_audioSource.clip = m_audioClip1;
        m_audioSource.Play();
    }
    public void DeadEvt()
    {
        m_audioSource.clip = m_audioClip2;
        m_audioSource.Play();
    }
}
