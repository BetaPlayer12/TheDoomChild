using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ObstacleChecker : MonoBehaviour
    {
        public BlackBloodFlood blackBloodFlood;
        [SerializeField]
        public bool isFloodingBlackBlood;
        public List<PoolableObject> monolithSlamObstacleList = new List<PoolableObject>();
        public List<GameObject> wallTentaclesList = new List<GameObject>();

        public bool isMonolithSlamObstaclePresent;
        public bool isWallTentaclesPresent;

        public event EventAction<EventActionArgs> MonolithAdded;
        public event EventAction<EventActionArgs> MonolithEmptied;
        public event EventAction<EventActionArgs> BlackBloodFloodActive;
        public event EventAction<EventActionArgs> BlackBloodFloodInActive;
        public event EventAction<EventActionArgs> WallTentaclesPresent;
        public event EventAction<EventActionArgs> WallTentaclesCleared;
        public event EventAction<EventActionArgs> ObstacleAdded;
        public event EventAction<EventActionArgs> ObstaclesCleared;

        private void Awake()
        {
            blackBloodFlood.FloodDone += BlackBloodFloodDone;
            blackBloodFlood.FloodStarted += BlackBloodFloodStarted;
        }

        private void BlackBloodFloodStarted(object sender, EventActionArgs eventArgs)
        {
            blackBloodFlood.isFlooding = true;
            BlackBloodFloodActive?.Invoke(this, EventActionArgs.Empty);
            ObstacleAdded?.Invoke(this, EventActionArgs.Empty);
        }

        private void BlackBloodFloodDone(object sender, EventActionArgs eventArgs)
        {
            blackBloodFlood.isFlooding = false;
            BlackBloodFloodInActive?.Invoke(this, EventActionArgs.Empty);
        }

        // Update is called once per frame
        void Update()
        {
            if (!isMonolithSlamObstaclePresent && !isFloodingBlackBlood && !isWallTentaclesPresent)
            {
                ObstaclesCleared?.Invoke(this, EventActionArgs.Empty);
                Debug.Log("Obstacles CLeared");
            }

            if(wallTentaclesList.Count > 0)
            {
                isWallTentaclesPresent = true;
            }
            else
            {
                isWallTentaclesPresent = false;
            }
        }

        public void AddMonolithToList(PoolableObject monolith)
        {
            monolithSlamObstacleList.Add(monolith);
            isMonolithSlamObstaclePresent = true;
            MonolithAdded?.Invoke(this, EventActionArgs.Empty);
            ObstacleAdded?.Invoke(this, EventActionArgs.Empty);
        }

        public void ClearMonoliths()
        {
            foreach(PoolableObject monolith in monolithSlamObstacleList)
            {
                monolith.DestroyInstance();
            }

            isMonolithSlamObstaclePresent = false;

            MonolithEmptied?.Invoke(this, EventActionArgs.Empty);
        }
    }
}

