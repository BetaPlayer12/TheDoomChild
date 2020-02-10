using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixerSnapshot playAmb;
    [SerializeField]
    private AudioMixerSnapshot playIdle;
    [SerializeField]
    private AudioMixerSnapshot playFalls;
    [SerializeField]
    private AudioMixerSnapshot playBossFight;
    [SerializeField]
    private AudioMixerSnapshot nearBossFight;



    

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Amb1"))
        {
            playAmb.TransitionTo(0.5f);
        }
        if (collision.CompareTag("Falls"))
        {
            playFalls.TransitionTo(1.5f);
        }
        if (collision.CompareTag("Fire"))
        {
            playBossFight.TransitionTo(1.5f);
        }
        if (collision.CompareTag("NearBoss"))
        {
            //StartCoroutine(FadeIn(audioSource, 1f));
            nearBossFight.TransitionTo(0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Amb1"))
        {
            playIdle.TransitionTo(0.5f);
        }
        if (collision.CompareTag("Falls"))
        {
            playIdle.TransitionTo(1.0f);
        }
        if (collision.CompareTag("Fire"))
        {
            playIdle.TransitionTo(1.0f);
        }
        if (collision.CompareTag("NearBoss"))
        {
            playIdle.TransitionTo(1.3f);
        }
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
