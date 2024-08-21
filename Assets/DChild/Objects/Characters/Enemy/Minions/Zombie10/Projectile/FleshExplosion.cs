using DChild;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshExplosion : MonoBehaviour
{
    /*   [SerializeField]
       private GameObject[] m_fleshExplosionPieces;*/
    [SerializeField]
    private GameObject m_fleshExplosionFX;
    private void Start()
    {
        var fleshPiecesToSpawn = Random.Range(3, 5);
        var force = 40f;
        for(int i = 0; i < fleshPiecesToSpawn; i++)
        {
           
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_fleshExplosionFX);
            //var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_fleshExplosionFX, gameObject.scene);
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            /*m_fleshExplosionPieces[i].SetActive(true);*/
            var rigidbody = instance.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(Random.Range(-4f, 4f),Random.Range(2f, 3f)).normalized;
            rigidbody.AddForce(randomDirection * force, ForceMode2D.Impulse);
        }
    }

}
