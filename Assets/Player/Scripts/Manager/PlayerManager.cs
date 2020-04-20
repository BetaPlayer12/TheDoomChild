﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class PlayerManager : MonoBehaviour
    {
        private Rigidbody2D body2d;
        private InputState inputState;
        private Jog jogBehavior;
        private Dock crouchBehavior;
        private WallStick wallStickBehavior;
        private WallGrab wallGrabBehavior;
        private LongJump longJumpBehavior;
        //private WallJump wallJumpBehavior;
        private Slash slashBehavior;
        private ShadowSlide dashBehavior;
        private GroundShaker groundShakerBehavior;
        private Thrust thrustBehavior;
        private Animator animator;
        private WallSlide wallSlideBehavior;
        private Idle idleBehavior;



        private CollisionState collisionState;


        public Animator capeAnimation;



        private void Awake()
        {
            body2d = GetComponentInParent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            collisionState = GetComponent<CollisionState>();
            inputState = GetComponent<InputState>();
            jogBehavior = GetComponent<Jog>();
            crouchBehavior = GetComponent<Dock>();
            wallStickBehavior = GetComponent<WallStick>();
            wallGrabBehavior = GetComponent<WallGrab>();
            longJumpBehavior = GetComponent<LongJump>();
            //wallJumpBehavior = GetComponent<WallJump>();
            slashBehavior = GetComponent<Slash>();
            dashBehavior = GetComponent<ShadowSlide>();
            thrustBehavior = GetComponent<Thrust>();
            groundShakerBehavior = GetComponent<GroundShaker>();
            wallSlideBehavior = GetComponent<WallSlide>();
            idleBehavior = GetComponent<Idle>();

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (crouchBehavior.crouching && !jogBehavior.jogging)
            {
                inputState.absValX = 0;
            }
            if (collisionState.grounded)
            {
                JogAnimationState(0);
            }

            if (inputState.absValX > 0 && !wallStickBehavior.groundWallStick)
            {
                JogAnimationState(1);
            }



            if (inputState.absValY > 0)
            {
                // jumping

            }

            if (crouchBehavior.crouching)
            {
                if (inputState.absValX > 0)
                {
                    capeAnimation.SetBool("CrouchMoving", true);
                }
                else
                {
                    capeAnimation.SetBool("CrouchMoving", false);
                    capeAnimation.SetBool("CrouchIdle", true);
                }

            }
            else
            {
                capeAnimation.SetBool("CrouchIdle", false);
            }

            if (wallStickBehavior.groundWallStick)
            {
                JogAnimationState(0);
            }

            if (dashBehavior.dashing)
            {
                crouchBehavior.crouching = false;
            }

            /*            if (wallJumpBehavior.jumpingOffWall)
                        {
                            animator.SetTrigger("WallJump");
                            wallStickBehavior.onWallDetected = false;
                        }*/

            if (thrustBehavior.thrustAttack)
            {
                animator.SetBool("Thrust", true);
                if (!thrustBehavior.chargingAttack && !thrustBehavior.thrustHasStarted)
                {
                    animator.SetTrigger("ThrustStart");

                }
                else if (thrustBehavior.chargingAttack && thrustBehavior.thrustHasStarted)
                {
                    animator.ResetTrigger("ThrustStart");
                    capeAnimation.SetBool("ThrustCharging", true);
                    animator.SetBool("ThrustCharge", true);


                }
                else if (!thrustBehavior.chargingAttack)
                {
                    capeAnimation.SetBool("ThrustCharging", false);
                    animator.SetBool("ThrustCharge", false);
                    capeAnimation.SetBool("ThrustImpact", true);
                    animator.SetTrigger("ThrustEnd");
                }

            }
            else
            {
                capeAnimation.SetBool("ThrustImpact", false);
                animator.ResetTrigger("ThrustEnd");
                animator.SetBool("Thrust", false);
            }

            if (wallSlideBehavior.onWallDetected && !collisionState.grounded)
            {
                animator.SetBool("UpHold", wallSlideBehavior.upHold);
                animator.SetBool("DownHold", wallSlideBehavior.downHold);
            }

            if (!collisionState.grounded)
            {
                crouchBehavior.crouching = false;
            }
            if (dashBehavior.shadowMode)
            {
                crouchBehavior.crouching = false;
            }
            

            WallGrabAnimationState(wallGrabBehavior.canLedgeGrab);
            CrouchAnimationState(crouchBehavior.crouching);
            GroundednessAnimationState(collisionState.grounded);
            VelocityYAnimationState(body2d.velocity.y);
            WallStickAnimationState(wallStickBehavior.onWallDetected);
            DashAnimationState(dashBehavior.dashing, dashBehavior.shadowMode);
            GroundShakerAnimationState(groundShakerBehavior.groundSmash);
            IdleAnimationModeState(idleBehavior.attackMode, idleBehavior.idleState);
        }

        void IdleAnimationModeState(bool value, int value1)
        {
            animator.SetBool("AttackMode", value);
            animator.SetInteger("IdleState", value1);
        }

        void GroundShakerAnimationState(bool value)
        {
            animator.SetBool("EarthShake", value);


        }

        void SlashAnimationState(bool value1, int value2, bool value3, bool value4)
        {
            animator.SetBool("Attack", value1);
            animator.SetInteger("AttackState", value2 + 1);
            animator.SetBool("UpHold", value3);

        }

        void DashAnimationState(bool value1, bool value2 )
        {
            animator.SetBool("Dash", value1);
            animator.SetBool("ShadowDash", value2);
        }

        void VelocityYAnimationState(float value)
        {

            animator.SetFloat("Yvelocity", value);
            if (value > 0.5f || value < -0.5f)
            {
                capeAnimation.SetBool("Jumping", true);
            }
            else
            {
                capeAnimation.SetBool("Jumping", false);
            }
        }

        void GroundednessAnimationState(bool value)
        {
            animator.SetBool("Grounded", value);

        }

        void CrouchAnimationState(bool value)
        {
            animator.SetBool("Crouch", value);


        }
        void JogAnimationState(int value)
        {
            animator.SetInteger("Jog", value);
            if (value != 0)
            {
                capeAnimation.SetBool("Jog", true);
            }
            else
            {
                capeAnimation.SetBool("Jog", false);
            }
        }

        void WallGrabAnimationState(bool value)
        {
            animator.SetBool("WallGrab", value);
        }

        void WallStickAnimationState(bool value)
        {
            animator.SetBool("WallStick", value);
        }

    }
}

