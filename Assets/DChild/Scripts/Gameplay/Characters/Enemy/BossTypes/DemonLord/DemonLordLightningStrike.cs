using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class DemonLordLightningStrike : PoolableObject
    {
        [SerializeField]
        private Animator m_animator;

        [Button]
        public void StrikeLightning()
        {
            m_animator.SetTrigger("LightningStrike");
        }
    }
}

