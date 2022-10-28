using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Projectiles
{
    public class TentacleLifeSpan : PoolableObject
    {
        public float m_timer;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;

            if (m_timer < 0)
            {
                DestroyInstance();
            }
        }
    }
}

