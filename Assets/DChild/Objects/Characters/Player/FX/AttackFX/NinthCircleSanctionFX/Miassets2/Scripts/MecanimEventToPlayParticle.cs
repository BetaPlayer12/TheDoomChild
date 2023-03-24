using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanimEventToPlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem; // Make the variable serializable
    public float particleDuration = 2f;

    private float particleTimer = 0f;
    private bool particlePlaying = false;

    void Update()
    {
        if (particlePlaying)
        {
            particleTimer += Time.deltaTime;
            if (particleTimer >= particleDuration)
            {
                particleSystem.Stop();
                particlePlaying = false;
            }
        }
    }

    void OnMecanimEvent()
    {
        particleSystem.Play();
        particlePlaying = true;
        particleTimer = 0f;
    }
}
