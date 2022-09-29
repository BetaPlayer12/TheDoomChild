using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay
{
    public class SavePointMimicSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] savePointMimics;

        [SerializeField, Range(0, 100)]
        private int mimicSpawnChance;

        private int spawnDiceRoll;
        private int mimicIndexRoll;

        private void Start()
        {
            rollSpawnChance();
        }

        public void rollSpawnChance()
        {
            mimicIndexRoll = Random.Range(0, savePointMimics.Length);
            spawnDiceRoll = Random.Range(0, 100);

            Debug.Log("Spawn roll: " + spawnDiceRoll + " mimic index: " + mimicIndexRoll);

            if(spawnDiceRoll <= mimicSpawnChance)
            {
                savePointMimics[mimicIndexRoll].SetActive(true);
            }
        }

#if UNITY_EDITOR
        [Button]
        private void rollSpawnChanceEditor()
        {
            mimicIndexRoll = Random.Range(0, savePointMimics.Length);
            spawnDiceRoll = Random.Range(0, 100);

            Debug.Log("Spawn roll: " + spawnDiceRoll + "mimic index: " + mimicIndexRoll);

            if (spawnDiceRoll <= mimicSpawnChance)
            {
                savePointMimics[mimicIndexRoll].SetActive(true);
            }
        }

        [Button]
        private void resetMimics()
        {
            foreach(GameObject mimic in savePointMimics)
            {
                mimic.SetActive(false);
            }
        }
#endif
    }
}

