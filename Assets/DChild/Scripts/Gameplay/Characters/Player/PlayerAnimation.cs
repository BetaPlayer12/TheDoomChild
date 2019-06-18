using DChild.Gameplay.Characters.Players.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class PlayerAnimation : CombatCharacterAnimation, IBasicAttackAnimation
    {
        private string m_currentAttackAnimation;

        public string currentAttackAnimation => m_currentAttackAnimation;

        #region New Jump, Fall and Land Animations
        public const string ANIMATION_JUMPNEW_START_RIGHT = "JumpNew_Start_Right";
        public const string ANIMATION_JUMPNEW_START_LEFT = "JumpNew_Start_Left";

        public const string ANIMATION_JUMPNEW_LOOP_RIGHT = "JumpNew_Loop_Right";
        public const string ANIMATION_JUMPNEW_LOOP_LEFT = "JumpNew_Loop_Left";

        public const string ANIMATION_JUMPNEW_FALL_START_RIGHT = "JumpNew to Fall1_Start_Right";
        public const string ANIMATION_JUMPNEW_FALL_START_LEFT = "JumpNew to Fall1_Start_Left";

        public const string ANIMATION_JUMPNEW_FALL_LOOP1_RIGHT = "JumpNew_Fall1_Loop_Right";
        public const string ANIMATION_JUMPNEW_FALL_LOOP1_LEFT = "JumpNew_Fall1_Loop_Left";

        public const string ANIMATION_JUMPNEW_FALL_LOOP2_RIGHT = "JumpNew_Fall2_Loop_Right";
        public const string ANIMATION_JUMPNEW_FALL_LOOP2_LEFT = "JumpNew_Fall2_Loop_Left";

        public const string ANIMATION_SHORTSTATIC_LANDING_RIGHT = "JumpNew_ShortStaticLanding_Right";
        public const string ANIMATION_SHORTSTATIC_LANDING_LEFT = "JumpNew_ShortStaticLanding_Left";

        public const string ANIMATION_HARD_LANDING_RIGHT = "HardLanding_Right";
        public const string ANIMATION_HARD_LANDING_LEFT = "HardLanding_Left";

        #endregion Animations

        #region Basic Animations
        //Left
        public const string ANIMATION_STAND_IDLE_LEFT = "Idle1_Left";
        public const string ANIMATION_POST_ATTACK_IDLE_LEFT = "Idle3_Left";
        public const string ANIMATION_ON_EDGE_IDLE_LEFT = "Idle4_Left";

        public const string ANIMATION_JOG_LEFT = "Jog_Static_Left";
        public const string ANIMATION_SPRINT_LEFT = "RunStatic_Left2";

        public const string ANIMATION_JUMP_STATIC_LEFT = "Jump2_Rising_Loop_Left";
        public const string ANIMATION_FALLING_STATIC_LEFT = "Jump2_Falling_Loop_Left";
        public const string ANIMATION_LANDING_STATIC_LEFT = "Jump2_Land_Left";

        public const string ANIMATION_JUMP_MOVING_LEFT = "Jump1_Rising_Left_NoStretch";
        public const string ANIMATION_FALLING_MOVING_LEFT = "Jump1_Falling_Left2";
        public const string ANIMATION_LANDING_MOVING_LEFT = "Jump1_Landing_Left2";

        public const string ANIMATION_FLINCH1_LEFT = "Flinch1_Left";
        public const string ANIMATION_FLINCH2_LEFT = "Flinch2_Left";
        public const string ANIMATION_FLINCH3_LEFT = "Flinch3_Left";

        public const string ANIMATION_CROUCH_IDLE_LEFT = "Crouch_Idle_Left";
        public const string ANIMATION_CROUCH_LEFT = "Crouch_Left";

        //Right
        public const string ANIMATION_STAND_IDLE_RIGHT = "Idle1_Right";
        public const string ANIMATION_POST_ATTACK_IDLE_RIGHT = "Idle3_Right";
        public const string ANIMATION_ON_EDGE_IDLE_RIGHT = "Idle4_Right";

        public const string ANIMATION_JOG_RIGHT = "Jog_Static_Right";
        public const string ANIMATION_SPRINT_RIGHT = "RunStatic_Right2";

        public const string ANIMATION_JUMP_STATIC_RIGHT = "Jump2_Rising_Loop_right";
        public const string ANIMATION_FALLING_STATIC_RIGHT = "Jump2_Falling_Loop_Right";
        public const string ANIMATION_LANDING_STATIC_RIGHT = "Jump2_Land_Right";

        public const string ANIMATION_JUMP_MOVING_RIGHT = "Jump1_Rising_Right_NoStretch";
        public const string ANIMATION_FALLING_MOVING_RIGHT = "Jump1_Falling_Right2";
        public const string ANIMATION_LANDING_MOVING_RIGHT = "Jump1_Landing_Right2";

        public const string ANIMATION_FLINCH1_RIGHT = "Flinch1_Right";
        public const string ANIMATION_FLINCH2_RIGHT = "Flinch2_Right";
        public const string ANIMATION_FLINCH3_RIGHT = "Flinch3_Right";

        public const string ANIMATION_CROUCH_IDLE_RIGHT = "Crouch_Idle_Right";
        public const string ANIMATION_CROUCH_RIGHT = "Crouch_Right";
        public const string ANIMATION_LEDGE_GRAB_RIGHT = "EdgeGrab_Right2";//
        #endregion

        #region Projectile
        //Left
        public const string ANIMATION_SKULLTHROW_CALL_LEFT = "SkullThrow_Call_Left";
        public const string ANIMATION_SKULLTHROW_AIM_LEFT = "SkullThrow_Aim_Left";   
        public const string ANIMATION_SKULLTHROW_ONHIT_LEFT = "SkullThrow_OnHit_Left";   

        //Right
        public const string ANIMATION_SKULLTHROW_CALL_RIGHT = "SkullThrow_Call_Right";
        public const string ANIMATION_SKULLTHROW_AIM_RIGHT = "SkullThrow_Aim_Right";
        public const string ANIMATION_SKULLTHROW_ONHIT_RIGHT = "SkullThrow_OnHit_Right";

        public const string EVENT_SKULLTHROW = "Projectile_Spawn_Skullthrow";   
        #endregion

        #region Skills Animations
        //Left
        public const string ANIMATION_DASH_LEFT = "Dash_Loop_Left";
        public const string ANIMATION_DOUBLEJUMP_ANTIC_LEFT = "JumpNew_DoubleJump_Start_Left";
        public const string ANIMATION_DOUBLEJUMP_RISE_LEFT = "DoubleJump_Rising_Left";
        public const string ANIMATION_DOUBLEJUMP_RISE2_LEFT = "DoubleJump_Rising2_Left";
        public const string ANIMATION_DOUBLEJUMP_RISE3_LEFT = "JumpNew_DoubleJump_Loop_Left";
        public const string ANIMATION_DOUBLEJUMP_FALL_LEFT = "JumpNew_DoubleJumpFall_Left";
        public const string ANIMATION_DOUBLEJUMP_LAND_LEFT = "DoubleJump_Landing4_Left";
        public const string ANIMATION_WALLSTICK_LEFT = "WallStickNew_Left";
        public const string ANIMATION_WALLSLIDE_LEFT = "WallSlideNew_Left";
        public const string ANIMATION_WALLJUMP_RISING_LEFT = "WallJump1_Rising_Left1";
        public const string ANIMATION_WALLJUMP_LEFT = "WallJumpNew_Loop_Left";

        //Right
        public const string ANIMATION_DASH_RIGHT = "Dash_Loop_Right";
        public const string ANIMATION_DOUBLEJUMP_ANTIC_RIGHT = "JumpNew_DoubleJump_Start_Right";
        public const string ANIMATION_DOUBLEJUMP_RISE_RIGHT = "DoubleJump_Rising_Right";
        public const string ANIMATION_DOUBLEJUMP_RISE2_RIGHT = "DoubleJump_Rising2_Right";
        public const string ANIMATION_DOUBLEJUMP_RISE3_RIGHT = "JumpNew_DoubleJump_Loop_Right";
        public const string ANIMATION_DOUBLEJUMP_FALL_START_RIGHT = "JumpNew_DoubleJump to Fall1_Start_Right";
        public const string ANIMATION_DOUBLEJUMP_FALL_RIGHT = "JumpNew_DoubleJumpFall_Right";
        public const string ANIMATION_DOUBLEJUMP_LAND_RIGHT = "DoubleJump_Landing4_Right";
        public const string ANIMATION_WALLSTICK_RIGHT = "WallStickNew_Right";
        public const string ANIMATION_WALLSLIDE_RIGHT = "WallSlideNew_Right";
        public const string ANIMATION_WALLJUMP_RISING_RIGHT = "WallJump1_Rising_Right1";
        public const string ANIMATION_WALLJUMP_RIGHT = "WallJumpNew_Loop_Right";

        #endregion

        #region Transitions
        //LEFT
        public const string ANIMATION_FALL_TO_JOG_LEFT = "Falling_To_Jog_Transition_Left2";
        public const string ANIMATION_IDLE_TO_DASH_LEFT = "Dash_Transition_Idle_To_Dash_Left";
        public const string ANIMATION_JOG_TO_DASH_LEFT = "Dash_Transition_Jog_To_Dash_Left";
        public const string ANIMATION_JUMP_STATIC_TO_DASH_LEFT = "Dash_Transition_Jump2_To_Dash_Left";
        public const string ANIMATION_JUMP_MOVING_TO_DASH_LEFT = "Dash_Transition_Jump1_To_Dash_Left";
        public const string ANIMATION_WALLJUMP_START_LEFT = "WallJump1_Start_Right1";
        public const string ANIMATION_WALLJUMP_END_LEFT = "WallJumpNew_Land_Left";

        //RIGHT
        public const string ANIMATION_FALL_TO_JOG_RIGHT = "Falling_To_Jog_Transition_Right2";
        public const string ANIMATION_IDLE_TO_DASH_RIGHT = "Dash_Transition_Idle_To_Dash_Right";
        public const string ANIMATION_JOG_TO_DASH_RIGHT = "Dash_Transition_Jog_To_Dash_Right";
        public const string ANIMATION_JUMP_STATIC_TO_DASH_RIGHT = "Dash_Transition_Jump2_To_Dash_Right";
        public const string ANIMATION_JUMP_MOVING_TO_DASH_RIGHT = "Dash_Transition_Jump1_To_Dash_Right";
        public const string ANIMATION_WALLJUMP_START_RIGHT = "WallJump1_Start_Right1";
        public const string ANIMATION_WALLJUMP_END_RIGHT = "WallJumpNew_Land_Right";
        #endregion

        #region Combat
        //Left
        public const string ANIMATION_ATTACK_FORWARD_LEFT = "Attack_Forward_Hit_Left1";
        public const string ANIMATION_ATTACK_CROUCH_LEFT = "Attack_Crouch_Hit_Left1";
        public const string ANIMATION_ATTACK_UPWARD_LEFT = "Attack_Upward_Hit_Left1";
        public const string ANIMATION_ATTACK_JUMP_UPWARD_LEFT = "JumpAttack_Upward_Left1";
        public const string ANIMATION_ATTACK_JUMP_DOWNWARD_LEFT = "JumpAttack_Downward_Left1";
        public const string ANIMATION_ATTACK_JUMP_FORWARD_LEFT = "JumpAttack_Left1";

        //Right
        public const string ANIMATION_ATTACK_FORWARD_RIGHT = "Attack_Forward_Hit_Right1";
        public const string ANIMATION_ATTACK_CROUCH_RIGHT = "Attack_Crouch_Hit_Right1";
        public const string ANIMATION_ATTACK_UPWARD_RIGHT = "Attack_Upward_Hit_Right1";
        public const string ANIMATION_ATTACK_JUMP_UPWARD_RIGHT = "JumpAttack_Upward_Right1";
        public const string ANIMATION_ATTACK_JUMP_DOWNWARD_RIGHT = "JumpAttack_Downward_Right1";
        public const string ANIMATION_ATTACK_JUMP_FORWARD_RIGHT = "JumpAttack_Right1";
        #endregion

        #region Idle States
        public void DoStandIdle(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_STAND_IDLE_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_STAND_IDLE_RIGHT, true);
            }
        }

        public void DoCrouchIdle(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_CROUCH_IDLE_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_CROUCH_IDLE_RIGHT, true);
            }
        }

        public void DoPostAttackIdle(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_POST_ATTACK_IDLE_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_POST_ATTACK_IDLE_RIGHT, true);
            }
        }

        public void DoOnEdgeIdle(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ON_EDGE_IDLE_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_ON_EDGE_IDLE_RIGHT, true);
            }
        }
        #endregion

        #region Moving States
        public void DoJog(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JOG_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_JOG_RIGHT, true);
            }
        }

        public void DoSprint(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_SPRINT_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_SPRINT_RIGHT, true);
            }
        }

        public void DoCrouchMove(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_CROUCH_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_CROUCH_RIGHT, true);
            }
        }

        #endregion

        #region Jump States

        public void DoDoubleJumpAntic(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_ANTIC_LEFT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_ANTIC_RIGHT, false);
            }
        }

        public void DoDoubleJump(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_RISE3_LEFT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_RISE3_RIGHT, false);
            }
        }

        public void DoStaticJump(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMP_STATIC_LEFT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMP_STATIC_RIGHT, false);
            }
        }

        public void DoMovingJump(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMP_MOVING_LEFT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMP_MOVING_RIGHT, false);
            }
        }

        #endregion

        #region Fall States
        public void DoStaticFall(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_FALLING_STATIC_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_FALLING_STATIC_RIGHT, true);
            }
        }

        public void DoMovingFall(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_FALLING_MOVING_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_FALLING_MOVING_RIGHT, true);
            }
        }

        public void DoDoubleJumpFallStart(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_FALL_START_RIGHT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_FALL_START_RIGHT, false);
            }
        }

        public void DoDoubleJumpFall(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_FALL_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_FALL_RIGHT, true);
            }
        }

        #endregion

        #region Land States
        public void DoStaticLand(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_SHORTSTATIC_LANDING_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_SHORTSTATIC_LANDING_RIGHT, false, 0);
            }
        }

        public void DoMovingLand(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_LANDING_MOVING_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_LANDING_MOVING_RIGHT, false, 0);
            }
        }

        public void DoDoubleJumpLand(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_LAND_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_DOUBLEJUMP_LAND_RIGHT, false, 0);
            }
        }

        public void DoFallToJog(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_FALL_TO_JOG_LEFT, false);
            }

            else
            {
                SetAnimation(0, ANIMATION_FALL_TO_JOG_RIGHT, false);
            }
        }

        public void DoHardLanding(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_HARD_LANDING_LEFT, false);
            }

            else
            {
                SetAnimation(0, ANIMATION_HARD_LANDING_RIGHT, false);
            }
        }

        #endregion

        #region Flinch States
        public void DoFlinchForward(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_FLINCH2_LEFT, false);
            }
            else if (direction == HorizontalDirection.Right)
            {
                SetAnimation(0, ANIMATION_FLINCH2_RIGHT, false);
            }
        }

        public void DoFlinchBack(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_FLINCH3_LEFT, false);
            }

            else if (direction == HorizontalDirection.Right)
            {
                SetAnimation(0, ANIMATION_FLINCH3_RIGHT, false);
            }
        }

        #endregion

        #region Dash States
        public void DoDash(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_DASH_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_DASH_RIGHT, false, 0);
            }
        }

        public void DoIdleToDash(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_IDLE_TO_DASH_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_IDLE_TO_DASH_RIGHT, false, 0);
            }
        }

        public void DoJogToDash(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JOG_TO_DASH_LEFT, false, 0);
                //AddAnimation(0, ANIMATION_DASH_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JOG_TO_DASH_RIGHT, false, 0);
                //AddAnimation(0, ANIMATION_DASH_RIGHT, false, 0);
            }
        }

        public void DoJumpStaticToDash(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMP_STATIC_TO_DASH_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMP_STATIC_TO_DASH_RIGHT, false, 0);
            }
        }

        public void DoJumpMovingToDash(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMP_MOVING_TO_DASH_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMP_MOVING_TO_DASH_RIGHT, false, 0);
            }
        }

        #endregion

        #region WallStick and WallJump States
        public void DoWallStick(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_WALLSTICK_LEFT, true, 0);           
            }
            else
            {
                SetAnimation(0, ANIMATION_WALLSTICK_RIGHT, true, 0);
            }
        }

        public void DoWallSlide(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_WALLSLIDE_LEFT, true, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_WALLSLIDE_RIGHT, true, 0);
            }
        }

        public void DoWallJumpStart(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_WALLJUMP_START_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_WALLJUMP_START_RIGHT, false, 0);
            }
        }

        public void DoWallJump(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_WALLJUMP_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_WALLJUMP_RIGHT, false, 0);
            }

        }

        public void DoWallJumpRising(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_WALLJUMP_RISING_LEFT, true, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_WALLJUMP_RISING_RIGHT, true, 0);
            }

        }

        public void DoWallJumpEnd(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_WALLJUMP_END_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_WALLJUMP_END_RIGHT, false, 0);
            }
        }

        #endregion

        #region Combat States
        public void DoBasicAttack(int comboIndex, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ATTACK_FORWARD_LEFT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_FORWARD_LEFT);
            }
            else
            {
                SetAnimation(0, ANIMATION_ATTACK_FORWARD_RIGHT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_FORWARD_RIGHT);
            }
        }

        public void DoCrouchAttack(int comboIndex, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ATTACK_CROUCH_LEFT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_CROUCH_LEFT);
            }
            else
            {
                SetAnimation(0, ANIMATION_ATTACK_CROUCH_RIGHT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_CROUCH_RIGHT);
            }
        }

        public void DoUpwardAttack(int comboIndex, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ATTACK_UPWARD_LEFT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_UPWARD_LEFT);
            }
            else
            {
                SetAnimation(0, ANIMATION_ATTACK_UPWARD_RIGHT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_UPWARD_RIGHT);
            }

        }

        public void DoJumpAttackUpward(int comboIndex, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ATTACK_JUMP_UPWARD_LEFT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_JUMP_UPWARD_LEFT);
            }
            else
            {
                SetAnimation(0, ANIMATION_ATTACK_JUMP_UPWARD_RIGHT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_JUMP_UPWARD_RIGHT);
            }
  
        }

        public void DoJumpAttackDownward(int comboIndex, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ATTACK_JUMP_DOWNWARD_LEFT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_JUMP_DOWNWARD_LEFT);
            }
            else
            {
                SetAnimation(0, ANIMATION_ATTACK_JUMP_DOWNWARD_RIGHT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_JUMP_DOWNWARD_RIGHT);
            }
  
        }

        public void DoJumpAttackForward(int comboIndex, HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_ATTACK_JUMP_FORWARD_LEFT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_JUMP_FORWARD_LEFT);
            }
            else
            {
                SetAnimation(0, ANIMATION_ATTACK_JUMP_FORWARD_RIGHT, false, 0);
                SetCurrentAttackAnimation(ANIMATION_ATTACK_JUMP_FORWARD_RIGHT);
            }
  
        }

        #endregion

        #region Projectile
        public void DoSkullThrowCall(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_SKULLTHROW_CALL_LEFT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_SKULLTHROW_CALL_RIGHT, false);
            }
        }

        public void DoSkullThrowAim(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_SKULLTHROW_AIM_LEFT, true);
            }
            else
            {
                SetAnimation(0, ANIMATION_SKULLTHROW_AIM_RIGHT, true);
            }
        }

        public void DoSkullThrowOnHit(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_SKULLTHROW_ONHIT_LEFT, false);
            }
            else
            {
                SetAnimation(0, ANIMATION_SKULLTHROW_ONHIT_RIGHT, false);
            }
        }

        #endregion

        #region New Jump Animations
        public void DoStartJump(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMPNEW_START_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMPNEW_START_RIGHT, false, 0);
            }
        }

        public void DoJumpLoop(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMPNEW_LOOP_LEFT, true, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMPNEW_LOOP_RIGHT, true, 0);
            }
        }

        public void DoFallStart(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMPNEW_FALL_START_LEFT, false, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMPNEW_FALL_START_RIGHT, false, 0);
            }
        }

        public void DoFallLoop1(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMPNEW_FALL_LOOP1_LEFT, true, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMPNEW_FALL_LOOP1_RIGHT, true, 0);
            }
        }

        public void DoFallLoop2(HorizontalDirection direction)
        {
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_JUMPNEW_FALL_LOOP2_LEFT, true, 0);
            }
            else
            {
                SetAnimation(0, ANIMATION_JUMPNEW_FALL_LOOP2_RIGHT, true, 0);
            }
        }
        #endregion

        public void DoLedgeGrab(HorizontalDirection direction)
        {
            Debug.Log("Animation play");
            if (direction == HorizontalDirection.Left)
            {
                SetAnimation(0, ANIMATION_LEDGE_GRAB_RIGHT, false, 0);
            }
            else
                SetAnimation(0, ANIMATION_LEDGE_GRAB_RIGHT, false, 0);//

        }

        public void SetCurrentAttackAnimation(string attackAnimation)
        {
            m_currentAttackAnimation = attackAnimation;
        }
    }
}