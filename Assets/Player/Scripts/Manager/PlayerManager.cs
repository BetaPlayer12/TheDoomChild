﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class PlayerManager : MonoBehaviour
    {
        private InputState inputState;
        private Jog jogBehavior;
        private Crouch crouchBehavior;
        private WallStick wallStickBehavior;
        private WallGrab wallGrabBehavior;
        private LongJump longJumpBehavior;
        private WallJump wallJumpBehavior;
        private Slash slashBehavior;
        private Dash dashBehavior;
        private GroundShaker groundShakerBehavior;
        private Animator animator;
        private CollisionState collisionState;




        private void Awake()
        {
            animator = GetComponent<Animator>();
            collisionState = GetComponent<CollisionState>();
            inputState = GetComponent<InputState>();
            jogBehavior = GetComponent<Jog>();
            crouchBehavior = GetComponent<Crouch>();
            wallStickBehavior = GetComponent<WallStick>();
            wallGrabBehavior = GetComponent<WallGrab>();
            longJumpBehavior = GetComponent<LongJump>();
            wallJumpBehavior = GetComponent<WallJump>();
            slashBehavior = GetComponent<Slash>();
            dashBehavior = GetComponent<Dash>();
            groundShakerBehavior = GetComponent<GroundShaker>();
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

            if (inputState.absValX > 0)
            {
                JogAnimationState(1);
            }

            if (inputState.absValY > 0)
            {
                // jumping

            }

            if (crouchBehavior.crouching)
            {

            }

            if (wallJumpBehavior.jumpingOffWall)
            {
                animator.SetTrigger("WallJump");
                wallStickBehavior.onWallDetected = false;
            }

            


            WallGrabAnimationState(wallGrabBehavior.canLedgeGrab);
            CrouchAnimationState(crouchBehavior.crouching);
            GroundednessAnimationState(collisionState.grounded);
            VelocityYAnimationState(Mathf.Floor(longJumpBehavior.velocityY));
            WallStickAnimationState(wallStickBehavior.onWallDetected);
            DashAnimationState(dashBehavior.dashing);
            SlashAnimationState(slashBehavior.attacking, slashBehavior.attackCounter);
            GroundShakerAnimationState(groundShakerBehavior.groundSmash);

        }

        void GroundShakerAnimationState(bool value)
        {
            animator.SetBool("EarthShake", value);
        }

        void SlashAnimationState(bool value1, int value2)
        {
            animator.SetBool("Attack", value1);
          
                animator.SetInteger("AttackState", value2 + 1);
        }

        void DashAnimationState(bool value)
        {
            animator.SetBool("Dash", value);
        }

        void VelocityYAnimationState(float value)
        {

            animator.SetFloat("Yvelocity", value);
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

