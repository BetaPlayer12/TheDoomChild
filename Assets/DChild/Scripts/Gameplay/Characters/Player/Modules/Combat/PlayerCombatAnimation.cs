using System;
using DChild.Gameplay.Characters.Players.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerCombatAnimation : MonoBehaviour/*SpineRootAnimation, IBasicAttackAnimation*/
    {
    //    private enum AttackType
    //    {
    //        Attack1,
    //        Attack2
    //    }
    //    [SerializeField]
    //    private AttackType m_attackType;

    //    private string m_currentAttackAnimation;

    //    public string currentAttackAnimation => m_currentAttackAnimation;

    //    public void DoBasicAttack(int comboIndex, Direction direction)
    //    {
    //        if (m_attackType == AttackType.Attack1)
    //        {
    //            if (direction == Direction.Left)
    //            {
    //                SetCombatAnimationForce($"Attack{comboIndex}_Left");
    //            }
    //            else
    //            {
    //                SetCombatAnimationForce($"Attack{comboIndex}_Right");
    //            }
    //        }

    //        else
    //        {
    //            if (direction == Direction.Left)
    //            {
    //                SetCombatAnimationForce("Attack_Forward_Hit_Left1");
    //            }
    //            else
    //            {
    //                SetCombatAnimationForce("Attack_Forward_Hit_Right1");
    //            }
    //        }
    //    }

    //    public void DoUpwardAttack(int comboIndex, Direction direction)
    //    {
    //        if (direction == Direction.Left)
    //        {
    //            SetCombatAnimationForce("Attack_Upward_Hit_Left1");
    //        }
    //        else
    //        {
    //            SetCombatAnimationForce("Attack_Upward_Hit_Right1");
    //        }
    //    }

    //    public void DoJumpAttack(int comboIndex, Direction direction)
    //    {
    //        if (direction == Direction.Left)
    //        {
    //            SetCombatAnimationForce("JumpAttack1_Left");
    //        }
    //        else
    //        {
    //            SetCombatAnimationForce("JumpAttack1_Right");
    //        }
    //    }

    //    public void DoCrouchAttack(int comboIndex, Direction direction)
    //    {
    //        if (direction == Direction.Left)
    //        {
    //            SetCombatAnimationForce("Attack_Crouch_Hit_Left1");
    //        }
    //        else
    //        {
    //            SetCombatAnimationForce("Attack_Crouch_Hit_Right1");
    //        }
    //    }

    //    public void SetCombatAnimationForce(string animation)
    //    {
    //        m_currentAttackAnimation = animation;
    //        SetAnimation(0, m_currentAttackAnimation, false, 0);
    //    }

    //    private void SetCombatAnimation(string animation)
    //    {
    //        m_currentAttackAnimation = animation;
    //        SetAnimation(0, m_currentAttackAnimation, false);
    //    }
    }
}