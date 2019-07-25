using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class AnimationController : MonoBehaviour, IPlayerExternalModule
    {
        private PlayerAnimation m_animation;

        private IFacing m_facing;
        private HorizontalDirection m_prevFacing;
        private ICombatState m_combatState;
        private IBehaviourState m_behaviourState;
        private IPlayerAnimationState m_animationState;

        private bool m_isThrowingBomb;
        private bool m_jumpLoop;
        private bool m_loopBreaker;
        

        public void Initialize(IPlayerModules player)
        {
            m_facing = player;
            m_animation = player.animation;
            m_combatState = player.characterState;
            m_animationState = player.animationState;
            m_behaviourState = player.characterState;
            m_prevFacing = m_facing.currentFacingDirection;
        }

        public void CallLateUpdate(IPlayerState state)
        {
            if (state.isLedging)
            {
             
            }

            #region Flinch
            if (state.isFlinching)
            {
               
                m_animationState.ResetAnimations();
                m_loopBreaker = false;
                Debug.Log("animation check");
                return;
            }
            #endregion

            #region Projectile
            else if (state.isAimingProjectile)
            {
                m_isThrowingBomb = true;
            }
            #endregion

            #region Attack
            else if (state.isAttacking)
            {

            }
            #endregion
            

            #region Skills
            else if (state.isDashing)
            {
                if (m_animationState.isFromIdle)
                {
                    m_animation.DoIdleToDash(m_facing.currentFacingDirection);
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        m_animationState.isFromIdle = false;
                    }
                }

                else if (m_animationState.isFromJog)
                {
                    m_animation.DoJogToDash(m_facing.currentFacingDirection);
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        m_animationState.isFromJog = false;
                    }
                }

                else if (m_animationState.hasJumped)
                {
                    if (m_animationState.isStaticJumping)
                    {
                        m_animation.DoJumpStaticToDash(m_facing.currentFacingDirection);
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animationState.hasJumped = false;
                        }
                    }

                    else
                    {
                        m_animation.DoJumpMovingToDash(m_facing.currentFacingDirection);
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animationState.hasJumped = false;
                        }
                    }
                }

                else
                    m_animation.DoDash(m_facing.currentFacingDirection);

                m_animationState.isLanding = false;
                m_animationState.hasDashed = true;
            }

            else if (state.isWallJumping)
            {
               

                if (state.isFalling)
                {
                   
                }
                else if (state.canDoubleJump)
                {
                    if (m_animationState.isWallJumping)
                    {
                        m_animation.DoWallJump(m_facing.currentFacingDirection);
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animationState.isWallJumping = false;
                            m_animationState.onWallJumpEnd = true;
                        }

                        m_animationState.isFallingFromWallJump = true;
                    }
                }

                else
                {
                    if (m_animationState.isAnticPlayed)
                    {
                        m_animation.DoDoubleJump(m_facing.currentFacingDirection);
                        m_animationState.hasDoubleJumped = true;
                        //m_animationState.isStaticJumping = false;
                    }

                    else
                    {
                        m_animation.DoDoubleJumpAntic(m_facing.currentFacingDirection);
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animationState.isAnticPlayed = true;
                        }
                    }
                }

                m_animationState.hasWallSticked = false;
            }

            else if (state.isSlidingToWall)
            {
                  m_animation.DoWallSlide(m_facing.currentFacingDirection);
                 m_animationState.isWallJumping = true;
                // m_animationState.isFallingFromWallJump = true;

                m_animationState.isFallingFromWallJump = true;
            }
            
            else if (state.isStickingToWall)
            {
                if (m_animationState.onWallJumpEnd)
                {
                    m_animation.DoWallJumpEnd(m_facing.currentFacingDirection);

                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        //m_animationState.onWallJumpEnd = false;
                    }
                }
                else
                {
                    m_animation.DoWallStick(m_facing.currentFacingDirection);
                }

                m_animationState.isWallJumping = true;
                m_animationState.hasWallSticked = true;
            }

            else if (state.hasDoubleJumped)
            {
                if (!state.isGrounded)
                {
                   

                    if (state.isFalling)
                    {
                       
                    }
                    else
                    {
                        if (m_animationState.isAnticPlayed)
                        {
                            m_animation.DoDoubleJump(m_facing.currentFacingDirection);
                            m_animationState.hasDoubleJumped = true;
                            //m_animationState.isStaticJumping = false;
                        }
                        else
                        {
                            m_animation.DoDoubleJumpAntic(m_facing.currentFacingDirection);
                            if (m_animation.animationState.GetCurrent(0).IsComplete)
                            {
                                m_animationState.isAnticPlayed = true;
                            }

                        }
                    }
                }
            }

            #endregion

            #region Basic Movements

           
            else if (state.isFalling)
            {
                

            }

            else if (state.canHighJump)
            {
                m_loopBreaker = true;

                if (state.hasDoubleJumped)
                {

                }

                else
                {
                   
                    
                        if (m_loopBreaker)
                        {
                            m_animation.DoJumpLoop(m_facing.currentFacingDirection);
                            m_loopBreaker = false;
                           
                        }


                       if(!m_loopBreaker){
                        m_animationState.ResetAnimations();
                    //    m_animation.DoStandIdle(m_facing.currentFacingDirection);
                        m_loopBreaker = true;
                        Debug.Log("Loop Breaker  is this looping");
                        return;
                       }
                      
                        
                       

                   
                   
                }
            }

            else if (state.isGrounded)
            {


                if (m_isThrowingBomb)
                {
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        m_isThrowingBomb = false;
                    }
                }

               


                else if (state.hasLanded)
                {
                    m_animationState.hasDashed = false;
                    m_animationState.hasJumped = false;
                    
                    

                }

                else if (state.isMoving)
                {
                    m_animationState.hasDashed = false;
                    m_combatState.isAttacking = false;
                    m_animationState.hasAttacked = false;
                    m_animationState.isLanding = false;
                    m_animationState.isFromIdle = false;
                    m_animationState.hasDoubleJumped = false;

                    if (m_animationState.hasWallSticked)
                    {
                        m_animationState.isLanding = true;
                    }

                    else if (state.isCrouched)
                    {
                        m_animation.DoCrouchMove(m_facing.currentFacingDirection);
                    }

                    else if (m_animationState.isFallingToJog)
                    {
                        if (m_facing.currentFacingDirection == HorizontalDirection.Left && m_animation.skeletonAnimation.AnimationName == "Falling_To_Jog_Transition_Right2")
                        {
                            m_animation.DoFallToJog(HorizontalDirection.Left);
                        }

                        else if (m_facing.currentFacingDirection == HorizontalDirection.Right && m_animation.skeletonAnimation.AnimationName == "Falling_To_Jog_Transition_Left2")
                        {
                            m_animation.DoFallToJog(HorizontalDirection.Right);
                        }

                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                            m_animationState.isFallingToJog = false;
                    }

                    else
                    {
                        if (state.isJogging)
                        {
                            m_animation.DoJog(m_facing.currentFacingDirection);
                            m_animationState.isFromJog = true;
                        }
                        else if (state.isSprinting)
                        {
                            m_animation.DoSprint(m_facing.currentFacingDirection);
                        }
                    }
                }

                else
                {
                    m_animationState.isFallingToJog = false;
                    m_animationState.isFromJog = false;
                    m_animationState.hasDoubleJumped = false;

                    if (m_animationState.isLanding)
                    {

                     
                        
                        if (m_animationState.isHardLanding)
                        {
                            
                            
                            m_animation.DoHardLanding(m_facing.currentFacingDirection);
                        }
                        else if (!state.isDashing && m_animationState.hasDashed)
                        {
                            m_animationState.hasDashed = false;
                            m_animation.DoStaticLand(m_facing.currentFacingDirection);
                        }

                        else
                        {
                            if (m_animation.animationState.GetCurrent(0).IsComplete)
                            {
                                m_animationState.isLanding = false;
                                m_animationState.hasWallSticked = false;
                                m_animationState.hasDoubleJumped = false;
                            }
                        }
                    }

                    else if (state.isCrouched)
                    {
                        m_animation.DoCrouchIdle(m_facing.currentFacingDirection);
                    }

                    else if (state.isNearEdge)
                    {
                        m_animation.DoOnEdgeIdle(m_facing.currentFacingDirection);
                    }

                    else if (m_animationState.hasAttacked)
                    {
                        m_animation.DoPostAttackIdle(m_facing.currentFacingDirection);
                    }

                    else if (m_animationState.isHardLanding)
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animationState.isHardLanding = false;
                            m_animationState.transitionToFall2 = false;
                        }
                    }

                    else
                    {
                        m_animation.DoStandIdle(m_facing.currentFacingDirection);
                    }

                    m_animationState.isShortJumping = false;
                    m_animationState.isFromIdle = true;
                }
            }

            #endregion

            m_prevFacing = m_facing.currentFacingDirection;
        }
       

    }

    
}