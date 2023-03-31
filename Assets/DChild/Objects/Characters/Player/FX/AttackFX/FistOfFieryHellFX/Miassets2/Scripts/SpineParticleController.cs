using UnityEngine;
using Spine.Unity;
using System.Collections;

public class SpineParticleController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public ParticleSystem particleSystem;
    public string startEventName;
    public string stopEventName;
    public float stopDelay = 0.5f;

    private bool isPlaying = false;

    private void Start()
    {
        //skeletonAnimation.state.Event += OnSpineEvent;
        skeletonAnimation.state.Event += State_Event;
    }

    private void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == startEventName && !isPlaying)
        {
            particleSystem.Play();
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
        particleSystem.Stop();
    }
}
