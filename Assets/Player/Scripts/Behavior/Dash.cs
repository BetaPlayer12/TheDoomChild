using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Dash : PlayerBehaviour
    {
        [SerializeField]
        private float m_dashVelocity;
        [SerializeField]
        private float m_dashCooldown;
        [SerializeField]
        private float m_dashDuration;

        public  float dashCooldownTimer;
        private PlayerMovement m_movement;

        protected virtual void Start()
        {
            dashCooldownTimer = m_dashCooldown;
            m_movement = GetComponent<PlayerMovement>();
        }

        protected virtual void FixedUpdate()
        {
            if (inputState.dashPressed && dashCooldownTimer <= 0)
            {
                if (stateManager.isFlinching == false && stateManager.isDashing == false && stateManager.isDead == false
                && (stateManager.onWall == false && stateManager.onWallLeg == false))
                {
                    StartCoroutine(DashRoutine());
                }
            }
            else if (dashCooldownTimer > 0 && (stateManager.isGrounded || stateManager.onWall))
            {
                dashCooldownTimer -= Time.deltaTime;
            }

            //float facingDir = facing.isFacingRight ? 1f : -1f;
            //var dash = inputState.GetButtonValue(inputButtons[0]);
            //var dashHold = inputState.GetButtonHoldTime(inputButtons[0]);

            //if (!canDash && lastDashTime > 0.1)
            //{
            //    lastDashTime -= Time.deltaTime;
            //    OnDash(facingDir);
            //}
            //else
            //{
            //    if (dashing)
            //    {
            //        StartCoroutine(FinishedDashRoutine());
            //    }
            //}

            //if (dash && dashHold < 0.1f && canDash)
            //{
            //    ToggleScripts(true);
            //    canDash = false;
            //    dashing = false;
            //    lastDashTime = dashTime;

            //    Debug.Log("Dash");
            //}
        }

        protected virtual void OnDash(float facingDir)
        {
            //var vel = rigidBody.velocity;
            //stateManager.isDashing = true;
            //stateManager.isIdle = false;
            //rigidBody.velocity = Vector2.zero;
            //rigidBody.AddForce(new Vector2(facingDir * dashForce, vel.y), ForceMode2D.Force);
            //dashing = true;
        }

        //IEnumerator FinishedDashRoutine()
        //{
        //    //dashing = false;
        //    //stateManager.isIdle = true;
        //    //stateManager.isDashing = false;
        //    yield return new WaitForSeconds(dashCoolDown);
        //    //canDash = true;
        //    //ToggleScripts(true);
        //}

        IEnumerator DashRoutine()
        {
            m_movement.DisableMovement();

            float direction = 0;
            stateManager.isDashing = true;
            stateManager.isIdle = false;
            stateManager.isAttacking = false;
            rigidBody.velocity = Vector2.zero;
            //rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

            direction = facing.isFacingRight ? (float)Directions.Right : (float)Directions.Left;

            Vector2 velocity = rigidBody.velocity;
            velocity.x = direction * m_dashVelocity;

            yield return null;

            rigidBody.AddForce(new Vector2(direction * m_dashVelocity, 0), ForceMode2D.Impulse);
            //rigidBody.velocity = velocity;
            dashCooldownTimer = m_dashCooldown;

            //Debug.Log("Dash");
            //Debug.Log("Velocity: " + rigidBody.velocity);

            float timer = m_dashDuration;
            do
            {
                if (stateManager.isFlinching)
                {
                    break;
                }
                else
                {
                    if (inputState.horizontal != 0)
                    {
                        if (Mathf.Sign(inputState.horizontal) != direction)
                        {
                            direction = inputState.horizontal;
                            playerMovement.FlipCharacterDirection();
                        }
                    }

                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(new Vector2(direction * m_dashVelocity, 0), ForceMode2D.Impulse);
                    timer -= GameplaySystem.time.deltaTime;
                    yield return null;
                }
            } while (timer > 0);

            //yield return new WaitForSeconds(0.1f);

            if (stateManager.isFlinching == false)
            {
                rigidBody.velocity = Vector2.zero;
                m_movement.EnableMovement();
            }

            stateManager.isDashing = false;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}