using UnityEngine;

public class AutoStopParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float stopDelay = 5.0f;

    private float timer;
    private bool isPlaying;

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

        isPlaying = false;
        timer = 0f;
    }

    private void Update()
    {
        if (particleSystem != null)
        {
            if (!isPlaying && particleSystem.isPlaying)
            {
                isPlaying = true;
            }

            if (isPlaying)
            {
                timer += Time.deltaTime;

                if (timer >= stopDelay && particleSystem.isPlaying)
                {
                    StopParticles();
                }
            }
        }
    }

    public void ForceStop(float delay = 0f)
    {
        if (particleSystem != null && particleSystem.isPlaying)
        {
            if (delay > 0f)
            {
                Invoke("StopParticles", delay);
            }
            else
            {
                StopParticles();
            }
        }
    }

    private void StopParticles()
    {
        particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Reset()
    {
        particleSystem.Clear();
        particleSystem.Play();
        isPlaying = true;
        timer = 0f;
    }
}
