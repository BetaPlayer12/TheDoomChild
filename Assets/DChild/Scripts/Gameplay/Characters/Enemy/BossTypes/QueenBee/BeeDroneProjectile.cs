using DChild.Gameplay.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeDroneProjectile : MonoBehaviour
{
    private SpineRootAnimation m_animation;

    public IEnumerator BeeRoutine()
    {
        yield return null;
    }

    private void Awake()
    {
        m_animation = GetComponent<SpineRootAnimation>();
    }
}
