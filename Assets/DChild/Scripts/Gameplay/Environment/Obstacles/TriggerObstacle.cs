using Holysoft;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Objects
{
    public abstract class TriggerObstacle : Actor
    {
        protected void OnTriggerStay2D(Collider2D col)
        {
            var hitBox = col.GetComponentInChildren<Hitbox>();
            if(hitBox != null)
            {
                SpawnObstacle();
            }
        }

        protected void OnTriggerExit2D(Collider2D col)
        {
            var hitBox = col.GetComponentInChildren<Hitbox>();
            if (hitBox != null)
            {
                UnSpawnObstacle();
            }
        }
    
        protected abstract void SpawnObstacle();
        protected abstract void UnSpawnObstacle();
    }
}
