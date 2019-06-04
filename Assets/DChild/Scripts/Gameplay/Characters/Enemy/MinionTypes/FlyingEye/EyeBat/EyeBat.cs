﻿using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyeBat : FlyingEye, ISpawnable
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_chaseSpeed;

        protected override CombatCharacterAnimation animation => null;

        public event EventAction<SpawnableEventArgs> Pool;
        public event EventAction<PoolItemEventArgs> PoolRequest;

        public void ForcePool()
        {
            throw new System.NotImplementedException();
        }

        public void Lunge()
        {

        }

        public override void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
        }

        public void Chase(ITarget target)
        {
            m_movement.MoveTo(target.position, m_chaseSpeed);
        }

        public void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public override void Turn()
        {
            TurnCharacter();
        }

        protected override IEnumerator FlinchRoutine()
        {
            throw new System.NotImplementedException();
        }

        public void DestroyItem()
        {
            Destroy(gameObject);
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }

}