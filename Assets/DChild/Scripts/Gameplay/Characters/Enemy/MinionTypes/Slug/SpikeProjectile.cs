using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Sirenix.Utilities;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeProjectile : PoolableObject, IDamageDealer, IAttacker
{
    [SerializeField]
    private AttackDamage m_damage;

    private const string ANIMATION_PROJECTILE = "Spike_Projectiles";

    private SkeletonAnimation m_animation;

    public event EventAction<CombatConclusionEventArgs> TargetDamaged;

    public void SpawnAt(Vector2 position, HorizontalDirection facing)
    {
        transform.position = position;
        var scale = Vector3.one;
        scale.x *= (int)facing;
        transform.localScale = scale;

        StartCoroutine(ProjectileSpawn());
    }

    private IEnumerator ProjectileSpawn()
    {
        m_animation.AnimationState.SetAnimation(0, ANIMATION_PROJECTILE, false);
        yield return new WaitForAnimationComplete(m_animation.AnimationState, ANIMATION_PROJECTILE);
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
        if (trackEntry.Animation.Name == ANIMATION_PROJECTILE)
        {
            CallPoolRequest();
        }
    }

    public void Damage(TargetInfo targetInfo, BodyDefense targetDefense)
    {
        if (!targetDefense.isInvulnerable)
        {
<<<<<<< HEAD
            AttackerInfo info = new AttackerInfo(transform.position, 0, 1, m_damage);
            var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
            TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, targetInfo, result));
=======
            //using (Cache<AttackerCombatInfo> info = Cache<AttackerCombatInfo>.Claim())
            //{
            //    info.Value.Initialize(transform.position, 0, 1, m_damage);
            //    var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
            //    TargetDamaged?.Invoke(this, new CombatConclusionEventArgs(info, targetInfo, result));
            //    info.Release();
            //}
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        }
    }
}

