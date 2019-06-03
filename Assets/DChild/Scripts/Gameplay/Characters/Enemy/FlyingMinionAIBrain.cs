using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pathfinding;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [RequireComponent(typeof(NavigationTracker))]
    public abstract class FlyingMinionAIBrain<T> : MinionAIBrain<T> where T : Minion, IMovingEnemy
    {
        protected NavigationTracker m_navigationTracker;

        protected virtual void MoveTo(Vector2 position)
        {
            if (m_navigationTracker.IsCurrentDestination(position))
            {
                var currentPath = m_navigationTracker.currentPathSegment;
                m_minion.MoveTo(currentPath);
            }
            else
            {
                m_navigationTracker.SetDestination(position);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_navigationTracker = GetComponent<NavigationTracker>();
        }
    }

}