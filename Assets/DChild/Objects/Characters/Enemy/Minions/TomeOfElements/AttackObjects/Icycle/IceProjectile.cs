using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class IceProjectile : MonoBehaviour
    {
        [SerializeField]
        private float m_lifespan;
        [SerializeField]
        private SimpleAttackProjectile m_simpleAttackProjectile; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (m_lifespan <= 0)
            {
                m_simpleAttackProjectile.DestroyInstance();
            }

            m_lifespan -= GameplaySystem.time.deltaTime;
        }


    }
}

