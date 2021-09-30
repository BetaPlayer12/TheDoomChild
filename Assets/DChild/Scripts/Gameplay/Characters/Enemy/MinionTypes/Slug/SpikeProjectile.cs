using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.Utilities;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class SpikeProjectile : PoolableObject, IDamageDealer, IAttacker
{
    [SerializeField]
    private Damage m_damage;

    private const string ANIMATION_PROJECTILE = "Spike_Projectiles";

    private SkeletonAnimation m_animation;

    public Vector2 position => transform.position;

    public Invulnerability ignoreInvulnerability => throw new System.NotImplementedException();

    public bool ignoresBlock => throw new System.NotImplementedException();

    public IAttacker parentAttacker => throw new System.NotImplementedException();

    public IAttacker rootParentAttacker => throw new System.NotImplementedException();

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

    public void SetParentAttacker(IAttacker damageDealer)
    {
        throw new System.NotImplementedException();
    }

    public void SetRootParentAttacker(IAttacker damageDealer)
    {
        throw new System.NotImplementedException();
    }

    public void Damage(TargetInfo target, Collider2D colliderThatDealtDamage)
    {
        throw new System.NotImplementedException();
    }
}

