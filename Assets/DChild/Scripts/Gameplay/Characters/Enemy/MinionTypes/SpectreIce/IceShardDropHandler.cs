using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardDropHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D[] m_shards;
    [SerializeField]
    public float m_dropDelay;
    private float delay = 0;
    IEnumerator DelayCoroutine(Rigidbody2D shard)
    {
        yield return new WaitForSeconds(delay);
        shard.gravityScale = 100;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_shards.Length; i++)
        {
            Rigidbody2D shard = m_shards[i];
            delay = m_dropDelay + delay;
            StartCoroutine(DelayCoroutine(shard));
        }
   }

   
}
