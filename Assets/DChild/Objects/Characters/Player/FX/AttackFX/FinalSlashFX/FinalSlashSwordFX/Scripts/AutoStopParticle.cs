using System.Collections;
using UnityEngine;

public class AutoStopParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float stopDelay = 5.0f;

    private void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
            if (particleSystem == null)
            {
                Debug.LogError("AutoStopParticle: No ParticleSystem found on the GameObject.");
                return;
            }
        }
    }

    public void CheckEffect()
    {
        StartCoroutine(StopEffectWithDelayRoutine());
    }

    private IEnumerator StopEffectWithDelayRoutine()
    {
        var timer = 0f;
        while (stopDelay > timer)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        StopParticles();
        yield return null;
    }

    public void ForceStop(float delay = 0f)
    {
        if (delay > 0f)
        {
            Invoke("StopParticles", delay);
        }
        else
        {

            StopAllCoroutines();
            StopParticles();
        }
    }

    private void StopParticles()
    {
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSystem.Stop();
    }

    public void Reset()
    {
        particleSystem.Clear();
        particleSystem.Play();
    }
}
