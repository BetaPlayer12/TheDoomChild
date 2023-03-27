using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanimEventToPlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems; // Make the variable serializable
    public float particleDuration = 2f;

    private float particleTimer = 0f;
    private bool particlePlaying = false;
    private int currentParticleSystemIndex = 0;

    void Update()
    {
        if (particlePlaying)
        {
            particleTimer += Time.deltaTime;
            if (particleTimer >= particleDuration)
            {
                particleSystems[currentParticleSystemIndex].Stop();
                particlePlaying = false;
            }
        }
    }

    void OnMecanimEvent(int particleSystemIndex)
    {
        if (particleSystemIndex >= 0 && particleSystemIndex < particleSystems.Length)
        {
            currentParticleSystemIndex = particleSystemIndex;
            particleSystems[currentParticleSystemIndex].Play();
            particlePlaying = true;
            particleTimer = 0f;
        }
    }
}
