using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay
{
    public class SavePointMimicSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_savePointMimics;

        [SerializeField, Range(0, 100)]
        private int m_mimicSpawnChance;

        private int m_spawnDiceRoll;
        private int m_mimicIndexRoll;

        private void Start()
        {
            ResetMimics();
            RollSpawnChance();
        }

        public void RollSpawnChance()
        {
            m_mimicIndexRoll = Random.Range(0, m_savePointMimics.Length);
            m_spawnDiceRoll = Random.Range(0, 100);

            Debug.Log("Spawn roll: " + m_spawnDiceRoll + " mimic index: " + m_mimicIndexRoll);

            if(m_spawnDiceRoll <= m_mimicSpawnChance)
            {
                m_savePointMimics[m_mimicIndexRoll].SetActive(true);
            }
        }

        [Button]
        private void RollSpawnChanceEditor()
        {
            m_mimicIndexRoll = Random.Range(0, m_savePointMimics.Length);
            m_spawnDiceRoll = Random.Range(0, 100);

            Debug.Log("Spawn roll: " + m_spawnDiceRoll + "mimic index: " + m_mimicIndexRoll);

            if (m_spawnDiceRoll <= m_mimicSpawnChance)
            {
                m_savePointMimics[m_mimicIndexRoll].SetActive(true);
            }
        }

        [Button]
        private void ResetMimics()
        {
            foreach(GameObject mimic in m_savePointMimics)
            {
                mimic.SetActive(false);
            }
        }
    }
}

