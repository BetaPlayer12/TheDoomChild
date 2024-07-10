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

        public ParticleSystem m_flameEffect;
        public GameObject m_flameCollider;
        public GameObject m_model;
        public Transform m_fireDragonMouthPos;

        [SerializeField, TabGroup("Reference")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Reference")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Reference")]
        private float m_moveOutOfWallSpeed;
        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, ValueDropdown("GetEvents"), TabGroup("Animation")]
        private string m_fireOnEvent;
        [SerializeField, ValueDropdown("GetEvents"), TabGroup("Animation")]
        private string m_fireOffEvent;
        private Vector2 m_spawnPosition;
        private Vector2 m_ToPlayerDirection;

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

        private void Awake()
        {
           m_spineEventListener.Subscribe(m_fireOnEvent, ShootFire);
           m_spineEventListener.Subscribe(m_fireOffEvent, OffFire);
        }

        private IEnumerator AttackRoutine()
        {
            m_spine.SetAnimation(0, m_attackAnimation, false);

            //THIS IS A TEMORARY FIXX
            /*yield return new WaitForSeconds(1.7f);
            ShootFire();
            yield return new WaitForSeconds(1f);
            OffFire();
            */

            yield return new WaitForAnimationComplete(m_spine.animationState, m_attackAnimation);
            DestroyInstance();
            yield return null;
        }

        public void ShootFire()
        {
            m_flameEffect.Play(true);
            m_flameCollider.SetActive(true);

        }

        void Update()
        {
                if (m_roofSensor.isDetecting)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector2(m_ToPlayerDirection.x, transform.position.y), m_moveOutOfWallSpeed);
                }
                if (m_wallSensor.isDetecting)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, m_ToPlayerDirection.y), m_moveOutOfWallSpeed);
                }
        }

        public void OffFire()
        {
            m_flameEffect.Stop(true);
            m_flameCollider.SetActive(false);
        }

        public void AdjustFireBreathRotationToPlayerPosition(Vector2 playerPosition)
        {
            Vector2 fireBreathPos = new Vector2(m_fireDragonMouthPos.position.x, m_fireDragonMouthPos.position.y);

            var toPlayer = playerPosition - fireBreathPos;
            var rad = Mathf.Atan2(toPlayer.y, toPlayer.x);

            //m_model.transform.localScale = new Vector3(transform.position.x < playerPosition.x ? -1 : 1, transform.position.y < playerPosition.y ? -1 : 1);
            
            m_model.transform.localScale = new Vector3(transform.position.x < playerPosition.x ? -1 : 1, 1);

            Debug.Log("Dragon Head: " + transform.position + "\n Dragon Scale: " + m_model.transform.localScale + "\n Player Pos: " + playerPosition);
        }

        public void SetPlayerPosition(Vector2 playerPos)
        {
            
            m_ToPlayerDirection = ((Vector2)transform.position - playerPos).normalized;
            m_spawnPosition = transform.position;
            m_playerPosition = playerPos;
            AdjustFireBreathRotationToPlayerPosition(m_playerPosition);
            PlayAttackAnimation();
            
        }

        [SerializeField, PreviewField, TabGroup("Animation")]
        protected SkeletonDataAsset m_skeletonDataAsset;

        //#if UNITY_EDITOR
        protected IEnumerable GetEvents()
        {
            ValueDropdownList<string> list = new ValueDropdownList<string>();
            var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Events.ToArray();
            for (int i = 0; i < reference.Length; i++)
            {
                list.Add(reference[i].Name);
            }
            return list;
        }
    }
}
