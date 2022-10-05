using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Holysoft.Collections;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class IceTrail : PoolableObject
    {
        [SerializeField]
        private float timer;

        // Update is called once per frame
        void Update()
        {
            timer -= GameplaySystem.time.deltaTime;

            if (timer < 0)
            {
                DestroyInstance();
            }

        }


    }
}

