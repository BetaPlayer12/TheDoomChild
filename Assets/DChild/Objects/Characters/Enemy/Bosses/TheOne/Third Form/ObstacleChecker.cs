using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ObstacleChecker : MonoBehaviour
    {
        public BlackBloodFlood blackBloodFlood;
        public bool isFloodingBlackBlood => blackBloodFlood.isFlooding;
        public List<PoolableObject> monolithSlamObstacleList = new List<PoolableObject>();
        public List<GameObject> wallTentacles = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ClearMonoliths()
        {
            foreach(PoolableObject monolith in monolithSlamObstacleList)
            {
                monolith.DestroyInstance();
            }
            monolithSlamObstacleList.Clear();
        }

        public void TurnOffFlood()
        {
            blackBloodFlood.isFlooding = false;
        }
    }
}

