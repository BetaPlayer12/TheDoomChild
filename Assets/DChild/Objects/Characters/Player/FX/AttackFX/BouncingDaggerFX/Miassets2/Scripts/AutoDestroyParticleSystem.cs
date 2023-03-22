using UnityEngine;

public class AutoDestroyParticleSystem : MonoBehaviour
{
    private new ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!particleSystem.IsAlive())
        {
            ParticleEffectPool effectPool = FindObjectOfType<ParticleEffectPool>();
            effectPool.ReturnToPool(gameObject);
        }
    }
}
