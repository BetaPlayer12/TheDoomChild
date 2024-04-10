using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class DemonLordLightningStrike : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        [Button]
        public void StrikeLightning()
        {
            m_animator.SetTrigger("LightningStrike");

        }
        
        public void StopAnimation()
        {
            m_animator.StopPlayback();
        }
        public void SetSpawnPosition(Vector3 spawnPoint)
        {
            transform.position = spawnPoint;
        }
    }
}

