using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem m_entranceFX;
    [SerializeField]
    protected ParticleSystem m_exitFX;

    public abstract Vector3 destination { get; }
    public abstract void PlayFX();
}
