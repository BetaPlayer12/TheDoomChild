using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Projectiles
{
    public class FireDragonHead : PoolableObject, IAimAtPlayer
    {
        private Vector2 m_playerPosition;
        [SerializeField]
        private SpineRootAnimation m_spine;

        public GameObject m_flameEffect;
        public GameObject m_flameCollider;
        public GameObject m_model;
        public Transform m_fireDragonMouthPos;

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
            m_flameEffect.SetActive(true);
            m_flameCollider.SetActive(true);
        }

        public void OffFire()
        {
            m_flameEffect.SetActive(false);
            m_flameCollider.SetActive(false);
        }

        public void AdjustFireBreathRotationToPlayerPosition(Vector2 playerPosition)
        {
            Vector2 fireBreathPos = new Vector2(m_fireDragonMouthPos.position.x, m_fireDragonMouthPos.position.y);

            var toPlayer = playerPosition - fireBreathPos;
            var rad = Mathf.Atan2(toPlayer.y, toPlayer.x);

            m_model.transform.localScale = new Vector3(transform.position.x < playerPosition.x ? -1 : 1, transform.position.y < playerPosition.y ? -1 : 1);

            Debug.Log("Dragon Head: " + transform.position + "\n Dragon Scale: " + m_model.transform.localScale + "\n Player Pos: " + playerPosition);
        }

        public void SetPlayerPosition(Vector2 playerPos)
        {
            m_playerPosition = playerPos;
            AdjustFireBreathRotationToPlayerPosition(m_playerPosition);
            PlayAttackAnimation();
        }
    }
}
