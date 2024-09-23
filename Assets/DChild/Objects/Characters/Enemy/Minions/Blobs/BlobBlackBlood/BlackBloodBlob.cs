using DChild.Gameplay.Characters;
using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Combat;
using Holysoft.Event;

public class BlackBloodBlob : MonoBehaviour
{
    [SerializeField, TabGroup("Reference")]
    private SpineRootAnimation m_animation;
    [SerializeField, TabGroup("Reference")]
    private Damageable m_damageable;
    [SerializeField, TabGroup("Reference")]
    private BreakableObject m_mainInstance;
    [SerializeField, TabGroup("Modules")]
    private MovementHandle2D m_movement;
    [SerializeField]
    private bool m_canMove;
    public bool canMove => m_canMove;

    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_resurrectionOfBlob;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_death;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_leftToDefault;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_rightToCeiling;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_ceilingToLeft;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_groundToright;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_leftWallPlatformIdle;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_rightWallPlatformIdle;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_patrolWalking;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_patrolWalkingLeftWallPlatformGoingDown;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_patrolWalkingLeftWallPlatformGoingUp;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_patrolWalkingRightWallPlatformGoingDown;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_patrolWalkingRightWallPlatformGoingUp;
    [SerializeField, Spine.Unity.SpineAnimation]
    private string m_idle;

    public event EventAction<EventActionArgs> Ressurected;
    public event EventAction<EventActionArgs> Death;
    [Button]
    public void BlobResurrection()
    {

        ResurrectionOfBlob();
    }
    [Button]
    public void BlobDeath()
    {

        Die();
    }
    [Button]
    public void GroundToRight()
    {

        StartCoroutine(GroundToRightRoutine());

    }
    [Button]
    public void CeilingToLeft()
    {
        StartCoroutine(CeilingToLeftRoutine());
    }
    [Button]
    public void LeftToDefault()
    {
        StartCoroutine(LeftToDefaultRoutine());
    }
    [Button]
    public void RightToCeiling()
    {
        StartCoroutine(RightToCeilingRoutine());
    }
    public void Die()
    {
        m_canMove = false;
        StartCoroutine(DeathRoutine());
    }
    public void DoMoveAnimation()
    {
        m_animation.SetAnimation(0, m_patrolWalking, true);
    }
    public void ResurrectionOfBlob()
    {
        gameObject.SetActive(true);
        m_mainInstance.SetObjectState(false);
        m_canMove = false;
        GameplaySystem.combatManager.Heal(m_damageable, 99999999);
        StartCoroutine(ResurrectionRoutine());
    }

    public IEnumerator ResurrectionRoutine()
    {

        m_animation.SetAnimation(0, m_resurrectionOfBlob, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_resurrectionOfBlob);
        m_canMove = true;
        m_animation.SetAnimation(0, m_idle, true);
        m_damageable.SetHitboxActive(true);
        Ressurected?.Invoke(this, EventActionArgs.Empty);

    }
    public IEnumerator DeathRoutine()
    {
        Death?.Invoke(this, EventActionArgs.Empty);
        m_animation.SetAnimation(0, m_death, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_death);
        gameObject.SetActive(false);

    }

    public IEnumerator CeilingToLeftRoutine()
    {
        m_animation.SetAnimation(0, m_ceilingToLeft, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_ceilingToLeft);
        m_animation.SetAnimation(0, m_idle, true);
    }

    public IEnumerator GroundToRightRoutine()
    {
        m_animation.SetAnimation(0, m_groundToright, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_groundToright);
        m_animation.SetAnimation(0, m_idle, true);
    }

    public IEnumerator LeftToDefaultRoutine()
    {
        m_animation.SetAnimation(0, m_leftToDefault, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_leftToDefault);
        m_animation.SetAnimation(0, m_idle, true);
    }
    public IEnumerator RightToCeilingRoutine()
    {
        m_animation.SetAnimation(0, m_rightToCeiling, false);
        yield return new WaitForAnimationComplete(m_animation.animationState, m_rightToCeiling);
        m_animation.SetAnimation(0, m_idle, true);
    }
}
