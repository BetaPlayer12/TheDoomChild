using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class StormCloud : PoolableObject
    {
        [SerializeField]
        private float m_timer;
        // Start is called before the first frame update
        void Start()
        {
            //CallPoolRequest();
        }

        // Update is called once per frame
        void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;

            if(m_timer < 0)
            {
                DestroyInstance();
            }
            
        }
    }

}