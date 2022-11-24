using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogAudio : MonoBehaviour
{
    #if UNITY_EDITOR
        [SerializeField]
        private SkeletonDataAsset m_asset;
#endif

    [SerializeField]
    private AudioClip m_audioClip;
    [SerializeField]
    private AudioClip m_audioClip2;
    [SerializeField]
    private AudioSource m_audioSource;



    Spine.EventData eventData;


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
            FootStepR();
            FootStepL();
        }
    }

    public void FootStepR()
    {
        m_audioSource.clip = m_audioClip;
        m_audioSource.Play();
    }
    public void FootStepL()
    {
        m_audioSource.clip = m_audioClip2;
        m_audioSource.Play();
    }
}
