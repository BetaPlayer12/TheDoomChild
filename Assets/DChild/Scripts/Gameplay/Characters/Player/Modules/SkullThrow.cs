using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullThrow : AttackBehaviour
{
    [SerializeField]
    private ProjectileInfo m_projectile;
    [SerializeField]
    private Transform m_spawnPoint;
    [SerializeField]
    private float m_skullThrowCooldown;

    private Character m_character;
    private ProjectileLauncher m_launcher;
    private int m_skullThrowAnimationParameter;

    public void HandleNextAttackDelay()
    {
        if (m_timer >= 0)
        {
            m_timer -= GameplaySystem.time.deltaTime;
            if (m_timer <= 0)
            {
                m_timer = -1;
                m_state.canAttack = true;
            }
        }
    }

    public void SpawnProjectile()
    {
        m_launcher.LaunchProjectile(new Vector2((float)m_character.facing, 0));
    }

    public override void AttackOver()
    {
        base.AttackOver();
        m_animator.SetBool(m_skullThrowAnimationParameter, false);
    }

    public void Execute()
    {
        m_timer = m_skullThrowCooldown;
        m_state.canAttack = false;
        m_state.isAttacking = true;
        m_state.waitForBehaviour = true;
        m_animator.SetBool(m_animationParameter, true);
        m_animator.SetBool(m_skullThrowAnimationParameter, true);
    }

    public override void Cancel()
    {
        base.Cancel();
        m_animator.SetBool(m_skullThrowAnimationParameter, false);
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void Initialize(ComplexCharacterInfo info)
    {
        base.Initialize(info);

        m_animator = info.animator;
        m_character = info.character;
        m_launcher = new ProjectileLauncher(m_projectile, m_spawnPoint);
        m_skullThrowAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SkullThrow);

        m_launcher.SetProjectile(m_projectile);
        m_launcher.SetSpawnPoint(m_spawnPoint);
    }
}
