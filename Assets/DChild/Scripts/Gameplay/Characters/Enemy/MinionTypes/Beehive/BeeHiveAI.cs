using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHiveAI : MonoBehaviour
{
    [SerializeField]
    private Transform m_spawnPoint;
    [SerializeField]
    private float m_spawnStartDelay;
    private bool m_spawn=false;
    [SerializeField, TabGroup("Summons")]
    private List<GameObject> m_minions;
    private List<ISummonedEnemy> m_summons;
   

    // Start is called before the first frame update
    public void SpawnBee()
    {

        for (int i = 0; i < m_minions.Count; i++)
        {
            if (!m_minions[i].gameObject.activeSelf)
            {
                m_summons[i].SummonAt(new Vector2(m_spawnPoint.position.x, m_spawnPoint.position.y ),null);
            }
       }
        m_spawn = true;
    }
    public void StopSpawn()
    {
        StopCoroutine(SpawnRoutine());
        m_spawn = false;
    }
    private IEnumerator SpawnRoutine()
    {
        m_spawn = false;
        yield return new WaitForSeconds(m_spawnStartDelay);
        SpawnBee();
    }
         void Awake()
    {
       
        m_summons = new List<ISummonedEnemy>();
        for (int i = 0; i < m_minions.Count; i++)
        {
            m_summons.Add(m_minions[i].GetComponent<ISummonedEnemy>());
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        if (m_spawn == true)
        {
            StartCoroutine(SpawnRoutine());
        }
        
    }
}
