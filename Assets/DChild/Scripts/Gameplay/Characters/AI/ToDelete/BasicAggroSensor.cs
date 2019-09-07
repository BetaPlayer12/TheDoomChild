using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class BasicAggroSensor : AggroSensor
    {
        private void OnTriggerStay2D(Collider2D collision)
        {
            var target = collision.GetComponentInParent<IEnemyTarget>();
            if (target != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                
                m_brain.SetTarget(target);
               
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var target = collision.GetComponentInParent<IEnemyTarget>();
            if (target != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_brain.SetTarget(null);
            }
        }
    }
}