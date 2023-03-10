using UnityEngine;
using Spine.Unity;
using System.Collections;

public class SpineParticleQueuer : MonoBehaviour
{
    public SkeletonAnimation[] skeletonAnimations;
    public ParticleSystem[] particleSystems;
    public string startEventName;
    public string stopEventName;
    public float stopDelay = 0.5f;

    private bool isPlaying = false;

    private void Start()
    {
        for (int i = 0; i < skeletonAnimations.Length; i++)
        {
            if (skeletonAnimations[i] != null)
            {
                skeletonAnimations[i].state.Event += State_Event;
            }
        }
    }

    private void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == startEventName && !isPlaying)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i] != null)
                {
                    particleSystems[i].Play();
                }
            }
            isPlaying = true;
        }
        else if (e.Data.Name == stopEventName && isPlaying)
        {
            StopParticleSystem();
        }
    }



    private void StopParticleSystem()
    {
        isPlaying = false;
        StartCoroutine(DelayedStop());
    }

    private IEnumerator DelayedStop()
    {
        yield return new WaitForSeconds(stopDelay);
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }
}