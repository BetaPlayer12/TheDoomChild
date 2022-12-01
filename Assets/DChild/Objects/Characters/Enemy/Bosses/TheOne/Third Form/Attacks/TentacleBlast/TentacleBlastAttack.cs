﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleBlastAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private TentacleBlast m_tentacleBlast;

        public IEnumerator ExecuteAttack()
        {
            yield return m_tentacleBlast.TentacleBlastAttack();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

