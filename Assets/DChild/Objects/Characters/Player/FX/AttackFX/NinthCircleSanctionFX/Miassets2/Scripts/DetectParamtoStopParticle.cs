using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectParamtoStopParticle : MonoBehaviour
{
    [SerializeField] private string paramName; //The name of the Animator Int Parameter to detect
    [SerializeField] private ParticleSystem[] particleSystems; //An array of particle systems to control
    [SerializeField] private float delay = 0.0f; //The time delay in seconds before stopping the particle systems

    private Animator anim;
    private bool stopParticles = false;
    private float stopTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        int paramValue = anim.GetInteger(paramName);

        if (paramValue == 1)
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }
            }
        }

        if (paramValue == 2 && !stopParticles)
        {
            stopParticles = true;
            stopTime = Time.time + delay;
        }

        if (stopParticles && Time.time >= stopTime)
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Stop();
            }
            stopParticles = false;
        }
    }
}
