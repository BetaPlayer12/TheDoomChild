using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Spine.Unity.Examples;
using UnityEngine;

public class ShadowSlide : MonoBehaviour, ISlide, IComplexCharacterModule
{
    [SerializeField]
    private Slide m_slide;
    [SerializeField, MinValue(0)]
    private int m_sourceRequiredAmount;
    [SerializeField]
    private ParticleSystem m_tempFX;

    private ICappedStat m_source;
    private Damageable m_damageable;
    private Animator m_animator;
    private bool m_wasUsed;
    private int m_animationParameter;
    private SkeletonGhost m_skeletonGhost;

    public void Initialize(ComplexCharacterInfo info)
    {
        m_source = info.magic;
        m_damageable = info.damageable;
        m_animator = info.animator;
        m_skeletonGhost = info.skeletonGhost;
        m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode);
    }

    public void Cancel()
    {
        m_slide.Cancel();
        GameplaySystem.world.SetShadowColliders(false);
        m_damageable.SetInvulnerability(Invulnerability.None);
        m_wasUsed = false;
        m_tempFX?.Stop(true);
        m_animator.SetBool(m_animationParameter, false);
        m_skeletonGhost.enabled = false;
    }

    public bool HaveEnoughSourceForExecution() => m_sourceRequiredAmount <= m_source.currentValue;

    public void ConsumeSource() => m_source.ReduceCurrentValue(m_sourceRequiredAmount);

    public void HandleCooldown() => m_slide.HandleCooldown();

    public void ResetCooldownTimer() => m_slide.ResetCooldownTimer();

    public void HandleDurationTimer() => m_slide.HandleDurationTimer();

    public bool IsSlideDurationOver() => m_slide.IsSlideDurationOver();

    public void ResetDurationTimer() => m_slide.ResetDurationTimer();

    public void Execute()
    {
        if (m_wasUsed == false)
        {
            GameplaySystem.world.SetShadowColliders(true);
            m_damageable.SetInvulnerability(Invulnerability.MAX);
            m_wasUsed = true;
            m_tempFX?.Play(true);
            m_animator.SetBool(m_animationParameter, true);
            //m_skeletonGhost.enabled = true;
        }

        m_slide.Execute();
    }

    public void Reset()
    {
        m_slide.Reset();
    }
}
