using System;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies.Collections
{
    public class GemSpike : PoolableObject
    {
        private SkeletonAnimation m_animation;

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_animation.AnimationState.SetAnimation(0, "attack", false);
        }

        public void SpawnAt(Vector2 position, Quaternion rotation, HorizontalDirection facing)
        {
            transform.position = position;
            transform.rotation = rotation;
            var scale = Vector3.one;
            scale.x *= (int)facing;
            transform.localScale = scale;
            m_animation.AnimationState.SetAnimation(0, "attack", false);
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Start()
        {
            m_animation.AnimationState.Complete += OnComplete;
            m_animation.AnimationState.SetAnimation(0, "attack", false);
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == "attack")
            {
                CallPoolRequest();
            }
        }
    }
}