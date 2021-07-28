using DChild.Gameplay;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        private SceneInfo m_sceneInfo;
        [SerializeField]
        private Location m_location;
        [SerializeField]
        private Vector2 m_spawnPosition;


        [Button]
        public void checkpoint()
        {
            GameplaySystem.campaignSerializer.slot.UpdateLocation(m_sceneInfo, m_location, m_spawnPosition);
            GameplaySystem.campaignSerializer.Save();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor")
            {
                checkpoint();


            }
        }
        private void Awake()
        {
            m_spawnPosition = transform.position;
        }


    }
}
