using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectDeathHandler : PoolableObject
{
    [SerializeField]
    public float m_despawnDelay;
    IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(m_despawnDelay);
        DestroyInstance();
    }
        // Start is called before the first frame update
        void Start()
    {
        StartCoroutine(DelayCoroutine());
    }

   
}
