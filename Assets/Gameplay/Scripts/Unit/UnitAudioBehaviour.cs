using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonCrashers
{
    public class UnitAudioBehaviour : MonoBehaviour
    {

        [Header("Data")]
        public UnitSFXData sfxData;

        [Header("Component Reference")]
        public AudioSource audioSource;

        [Header("SFX Volume Override")]
        public float sfxDeathVolume;

        public void PlaySFXGetHit()
        {
            PlayAudioClip(sfxData.GetGetHitClip());
        }

        public void PlaySFXDeath()
        {  
            SetAudioSourceVolume(sfxDeathVolume);
            PlayAudioClip(sfxData.GetDeathClip());
        }

        void SetAudioSourceVolume(float newVolume)
        {
            audioSource.volume = newVolume;
        }

        void PlayAudioClip(AudioClip selectedAudioClip)
        {
            audioSource.PlayOneShot(selectedAudioClip);
        }
    }
}

