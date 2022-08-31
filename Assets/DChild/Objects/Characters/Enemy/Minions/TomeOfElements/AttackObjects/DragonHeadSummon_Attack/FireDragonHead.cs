using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Characters.Enemies
{
    public class FireDragonHead : MonoBehaviour
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
            Debug.Log("Attack Animation Played");
            StartCoroutine(AttackRoutine());
        }

        public IEnumerator AttackRoutine()
        {
            Debug.Log("Attack Routine run");
            m_spine.SetAnimation(0, m_attackAnimation, false);
            Debug.Log("animation state: " + m_attackAnimation);
            yield return new WaitForAnimationComplete(m_spine.animationState, m_attackAnimation);
            gameObject.SetActive(false);
            yield return null;
        }

        public void ShootFire()
        {
            Debug.Log("Fire shot");
            flameEffect.SetActive(true);
            flameCollider.SetActive(true);
        }

        public void OffFire()
        {
            Debug.Log("Fire turned off");
            flameEffect.SetActive(false);
            flameCollider.SetActive(false);
        }

        private void Start()
        {
            
        }
    }
}
