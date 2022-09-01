using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;


namespace DChild.Gameplay.Projectiles
{
    public class FireDragonHead : PoolableObject
    {
        [SerializeField]
        private SpineRootAnimation m_spine;

        public GameObject flameEffect;
        public GameObject flameCollider;

        public void InitializeField(SpineRootAnimation spineRoot)
        {
            m_spine = spineRoot;
        }

        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonFAnimation")]
        private string m_attackAnimation;

        public void PlayAttackAnimation()
        {
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            m_spine.SetAnimation(0, m_attackAnimation, false);
            yield return new WaitForAnimationComplete(m_spine.animationState, m_attackAnimation);
            DestroyInstance();
            yield return null;
        }

        public void ShootFire()
        {
            Debug.Log("Fire shot");
            //StartCoroutine(ShootFireRoutine());
            flameEffect.SetActive(true);
            flameCollider.SetActive(true);
        }

        //private IEnumerator ShootFireRoutine()
        //{
        //    flameEffect.SetActive(true);
        //    flameCollider.SetActive(true);
        //}

        public void OffFire()
        {
            Debug.Log("Fire turned off");
            flameEffect.SetActive(false);
            flameCollider.SetActive(false);
        }

        private void Start()
        {
            PlayAttackAnimation();
        }
    }
}
