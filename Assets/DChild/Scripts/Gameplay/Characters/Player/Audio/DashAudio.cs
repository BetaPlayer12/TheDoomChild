﻿using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAudio : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private SkeletonDataAsset m_asset;
#endif

    [SerializeField]
    private AudioClip m_audioClip;

    [SerializeField]
    private AudioSource m_audioSource;



    Spine.EventData eventData;
    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<SkeletonAnimation>();


        animation.AnimationState.Event += HandleAnimationStateEvent;
    }

    private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {

        bool eventMatch = (eventData == e.Data);
        if (eventMatch)
        {
            DashEvt();
        }
    }

    public void DashEvt()
    {
        m_audioSource.clip = m_audioClip;
        m_audioSource.Play();
    }
}
