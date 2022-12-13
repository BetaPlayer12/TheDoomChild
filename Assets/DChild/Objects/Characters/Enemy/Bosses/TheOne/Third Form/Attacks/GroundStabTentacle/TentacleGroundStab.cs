﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters;
using Spine.Unity;
using DChild.Gameplay.Characters.Enemies;

namespace DChild.Gameplay.Projectiles
{
    public class TentacleGroundStab : PoolableObject
    {
        public float lifespan;
        public bool isOnPlayableGround = false;

        [SerializeField]
        private GameObject[] safeZones;
        [SerializeField]
        private Collider2D m_hitbox;

        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationLoopAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationStartAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_retractAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_stayAnimation;

        public IEnumerator StabRoutine()
        {
            m_animation.SetAnimation(0, m_anticipationStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationStartAnimation);

            m_animation.SetAnimation(0, m_attackAnimation, false);

            if(isOnPlayableGround)
                m_hitbox.enabled = true;

            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackAnimation);

            FindObjectOfType<ObstacleChecker>().ClearMonoliths();

            yield return TentacleStay();
        }

        public IEnumerator TentacleStay()
        {
            InitializeSafeZone();
            m_animation.SetAnimation(0, m_stayAnimation, false);
            yield return new WaitForSeconds(lifespan);
            yield return Retract();
        }

        public IEnumerator Retract()
        {
            RemoveSafeZones();
            m_animation.SetAnimation(0, m_retractAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_retractAnimation);
            
            DestroyInstance();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_hitbox.enabled = false;
            StartCoroutine(StabRoutine());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void InitializeSafeZone()
        {
            int randomSafeZone = Random.Range(0, safeZones.Length);

            GameObject safezone = safeZones[randomSafeZone];
            safezone.SetActive(true);
        }

        private void RemoveSafeZones()
        {
            foreach (GameObject safeZone in safeZones)
            {
                safeZone.SetActive(false);
            }
        }
    }
}

