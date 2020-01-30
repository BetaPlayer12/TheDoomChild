using Holysoft.Collections;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField, MinValue(1)]
        private int m_maxSpawns;
        [SerializeField]
        private RangeFloat m_spawnInterval;
        [SerializeField]
        private GameObject[] m_possibleSpawns;

        private float m_spawnTimer;
        private void SpawnCharacter()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {

        }

        private void LateUpdate()
        {
            if (m_spawnTimer <= 0)
            {
                SpawnCharacter();
                m_spawnTimer = m_spawnInterval.GenerateRandomValue();
            }
        }
    }
}