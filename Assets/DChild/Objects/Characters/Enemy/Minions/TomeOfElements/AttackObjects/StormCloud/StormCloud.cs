﻿using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class StormCloud : PoolableObject
    {
        public float timer;
        // Start is called before the first frame update
        void Start()
        {
            //CallPoolRequest();
        }

        // Update is called once per frame
        void Update()
        {
            timer -= GameplaySystem.time.deltaTime;

            if(timer < 0)
            {
                DestroyInstance();
            }
            
        }
    }

}