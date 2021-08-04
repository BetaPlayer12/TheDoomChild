using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies.Collections
{
    public class PotofGold : PoolableObject, IDamageDealer, IAttacker
    {
        [SerializeField]
        private RaySensor m_sensor;

        [SerializeField]
        private AttackDamage m_damage;

        private SkeletonAnimation m_animation;
        public event EventAction<CombatConclusionEventArgs> TargetDamaged;

        private const string ANIMATION_SPAWN = "Pot_of_Gold_Drop_Start";
        private const string ANIMATION_FALL = "Pot_of_Gold_Fall";
        private const string ANIMATION_DROP = "Pot_of_Gold_Fall_End";

        private bool m_waitForBehaviour;
        private bool m_hasLanded;

        public Vector2 position => transform.position;

        public Invulnerability ignoreInvulnerability => throw new System.NotImplementedException();

        public bool ignoresBlock => throw new System.NotImplementedException();

        public IAttacker parentAttacker => throw new System.NotImplementedException();

        public IAttacker rootParentAttacker => throw new System.NotImplementedException();

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_animation.AnimationState.SetAnimation(0, ANIMATION_SPAWN, false);
            StartCoroutine(SpawnRoutine());
        }

        public void SpawnAt(Vector2 position)
        {
            transform.position = position;
            var scale = Vector3.one;
            transform.localScale = scale;

            StartCoroutine(SpawnRoutine());        
        }

        private IEnumerator SpawnRoutine()
        {
            m_waitForBehaviour = true;
            m_animation.AnimationState.SetAnimation(0, ANIMATION_SPAWN, false);
            yield return new WaitForAnimationComplete(m_animation.AnimationState, ANIMATION_SPAWN);
            m_waitForBehaviour = false;
            yield break;
        }

        private IEnumerator DropRoutine()
        {
            m_waitForBehaviour = true;
            m_animation.AnimationState.SetAnimation(0, ANIMATION_DROP, false);
            yield return new WaitForAnimationComplete(m_animation.AnimationState, ANIMATION_DROP);
            m_hasLanded = true;
            m_waitForBehaviour = false;
            yield break;
        }

        public void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
        {
            if (targetDefense.invulnerabilityLevel == Invulnerability.None)
            {
                //using (Cache<AttackerCombatInfo> info = Cache<AttackerCombatInfo>.Claim())
                //{
                //    info.Value.Initialize(transform.position, 0, 1, m_damage);
                //    var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                //    TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, targetInfo, result));
                //    info.Release();
                //}
            }
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Start()
        {
            m_animation.AnimationState.Complete += OnComplete;
            m_waitForBehaviour = true;
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == ANIMATION_DROP)
            {
                CallPoolRequest();
            }
        }

        private void Update()
        {
            if (m_waitForBehaviour)
                return;

            m_sensor.Cast();

            if (m_hasLanded)
            {
                m_hasLanded = false;
                m_waitForBehaviour = true;
            }
            else if (m_sensor.isDetecting)
            { 
                StartCoroutine(DropRoutine());
            }
            else
            {
                m_animation.AnimationState.AddAnimation(0, ANIMATION_FALL, true, 0.2f);
            }

        }

        public void SetParentAttacker(IAttacker damageDealer)
        {
            throw new System.NotImplementedException();
        }

        public void SetRootParentAttacker(IAttacker damageDealer)
        {
            throw new System.NotImplementedException();
        }
        //# if UNITY_EDITOR
        //        [Button("Drop")]
        //        private void Drop()
        //        {
        //            this.gameObject.SetActive(true);
        //            SpawnAt(Vector2.zero);
        //        }
        //#endif
    }
}
