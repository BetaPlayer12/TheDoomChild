using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    [SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
    

    [Space]
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float basePitch = 1f;
    public float randomPitchOffset = 0.1f;

    Spine.EventData eventData;

    void OnValidate()
    {
        if (skeletonAnimation == null) GetComponent<SkeletonAnimation>();
        if (audioSource == null) GetComponent<AudioSource>();
    }

    void Start()
    {
        if (audioSource == null) return;
        if (skeletonAnimation == null) return;
        skeletonAnimation.Initialize(false);
        if (!skeletonAnimation.valid) return;

        
        skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
    }

    private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {
       
        bool eventMatch = (eventData == e.Data); 
        if (eventMatch)
        {
            PlayFootSteps();
            
        }
    }

    public void PlayFootSteps()
    {
        
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}

