using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSporeBulbHandle : MonoBehaviour
{
    [SerializeField]
    private MotherMantisAI m_mothermantisAI;
    [SerializeField]
    private List<Transform> m_patterns;
    [SerializeField]
    private List<Transform> m_pattern1;
    [SerializeField]
    private List<Transform> m_pattern2;
    [SerializeField]
    private GameObject m_sporeProjectile;


    private void SpawnSeeds()
    {
        if (m_sporeProjectile != null)
        {
            for (int x = 0; x < m_pattern1.Count; x++)
            {
                Instantiate(m_sporeProjectile, m_pattern1[x]);
            }
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
