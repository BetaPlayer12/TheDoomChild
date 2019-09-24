using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioMixerSnapshot playAmb;
    [SerializeField]
    private AudioMixerSnapshot playIdle;
    [SerializeField]
    private AudioMixerSnapshot playFalls;
    [SerializeField]
    private AudioMixerSnapshot playBossFight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Amb1"))
        {
            playAmb.TransitionTo(0.5f);
        }
        if (collision.CompareTag("Falls"))
        {
            playFalls.TransitionTo(0.5f);
        }
        if (collision.CompareTag("Fire"))
        {
            playBossFight.TransitionTo(0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("2d collision off: " + collision.transform.name);
        if (collision.CompareTag("Amb1"))
        {
            playIdle.TransitionTo(0.5f);
        }
        if (collision.CompareTag("Falls"))
        {
            playIdle.TransitionTo(0.5f);
        }
        if (collision.CompareTag("Fire"))
        {
            playIdle.TransitionTo(0.5f);
        }
    }
}
