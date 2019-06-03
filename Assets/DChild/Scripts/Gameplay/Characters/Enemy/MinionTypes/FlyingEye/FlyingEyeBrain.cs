﻿using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pathfinding;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [RequireComponent(typeof(WayPointPatroler),typeof(NavigationTracker))]
    public abstract class FlyingEyeBrain<T> : FlyingMinionAIBrain<T> where T : FlyingEye
    {
        private WayPointPatroler m_patrol;

        protected void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;
            if (m_navigationTracker.IsCurrentDestination(destination))
            {
                var currentPath = m_navigationTracker.currentPathSegment;
                if (IsLookingAt(currentPath))
                {
                    m_minion.MoveTo(currentPath);
                }
                else
                {
                    m_minion.Turn();
                }
            }
            else
            {
                m_navigationTracker.SetDestination(destination);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_navigationTracker = GetComponent<NavigationTracker>();
        }
    }

}