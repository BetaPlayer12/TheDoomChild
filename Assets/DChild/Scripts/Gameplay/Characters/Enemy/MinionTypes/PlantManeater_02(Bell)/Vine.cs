using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Characters.Enemies.Collections
{
    public class Vine : PoolableObject, IDamageDealer, IAttacker
    {
        [SerializeField]
        private AttackDamage m_damage;

        private const string ANIMATION_SPAWNVINE = "Attack1_Revised";
        private SkeletonAnimation m_animation;

        public Vector2 position => transform.position;
      
        public event EventAction<CombatConclusionEventArgs> TargetDamaged;

        public void Damage(ITarget target, BodyDefense targetDefense)
        {
            if (!targetDefense.isInvulnerable)
            {
                AttackInfo info = new AttackInfo(transform.position, 0, 1, m_damage);
                var result = GameplaySystem.combatManager.ResolveConflict(info, new TargetInfo(target, targetDefense.damageReduction));
                TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, target, result));
            }
        }

        public void SpawnAt(Vector2 position, HorizontalDirection facing)
        {
            transform.position = position;
            var scale = Vector3.one;
            scale.x *= (int)facing;
            transform.localScale = scale;

            StartCoroutine(SpawnVineRoutine());
        }

        private IEnumerator SpawnVineRoutine()
        {
            m_animation.AnimationState.SetAnimation(0, ANIMATION_SPAWNVINE, false);
            yield return new WaitForAnimationComplete(m_animation.AnimationState, ANIMATION_SPAWNVINE);
            yield return null;
        }

        private void Awake()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Start()
        {
            m_animation.AnimationState.Complete += OnComplete;
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == ANIMATION_SPAWNVINE)
            {
                GameSystem.poolManager.GetOrCreatePool<PoolableObjectPool>().AddToPool(this);
            }
        }
    }
}
