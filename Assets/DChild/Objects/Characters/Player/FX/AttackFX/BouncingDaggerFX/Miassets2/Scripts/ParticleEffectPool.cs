using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPool : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private int poolSize;

    private Queue<GameObject> pooledEffects;

    private void Awake()
    {
        pooledEffects = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject effectInstance = Instantiate(effectPrefab, transform);
            effectInstance.SetActive(false);
            pooledEffects.Enqueue(effectInstance);
        }
    }

    public GameObject GetPooledEffect()
    {
        if (pooledEffects.Count > 0)
        {
            GameObject effectInstance = pooledEffects.Dequeue();
            effectInstance.transform.SetParent(null);
            effectInstance.SetActive(true);
            return effectInstance;
        }

        return Instantiate(effectPrefab);
    }

    public void ReturnToPool(GameObject effectInstance)
    {
        effectInstance.SetActive(false);
        effectInstance.transform.SetParent(transform);
        pooledEffects.Enqueue(effectInstance);
    }
}
