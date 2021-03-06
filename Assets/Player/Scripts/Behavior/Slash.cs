﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Slash : PlayerBehaviour
    {

        private float timeBtwnAtck;
        private float attackTimeCounter;
        public float attackingTime;
        public int attackCounter;
        private float attackHold;
        private float comboTimer;
        private Dash dashState;

        public float startTimeBtwAttck;

        public bool attackHolding;
        public bool attacking;
        public bool upHold;
        // public float attackHold = 0.5f;
        public int comboCount;
        public float resetTime;

        public Transform attackPos;
        public Collider2D attackCollider;
        public LayerMask whatIsEnemies;
        public float attackRange;
        public bool holdingAttack;

        private Crouch crouchState;
        [SerializeField]
        private ParticleSystem m_forwardSlash1FX;
        [SerializeField]
        private ParticleSystem m_swordCombo1FX;
        [SerializeField]
        private ParticleSystem m_swordCombo2FX;
        [SerializeField]
        private ParticleSystem m_VFX_CrouchSlashX;
        [SerializeField]
        private ParticleSystem m_VFX_JumpUpSlashFX;
        [SerializeField]
        private ParticleSystem m_VFX_SwordUpSlashFX;
        [SerializeField]
        private ParticleSystem m_VFX_JumpSwordDownSlashFX;
        [SerializeField]
        private ParticleSystem m_VFX_SwordJumpSlashForward;
        [SerializeField]
        private Collider2D m_forwardSlashAttackCollider;
        [SerializeField]
        private Collider2D m_swordCombo1AttackCollider;
        [SerializeField]
        private Collider2D m_swordCombo2AttackCollider;
        [SerializeField]
        private Collider2D m_crouchSlashAttackCollider;
        [SerializeField]
        private Collider2D m_jumpSlashAttackCollider;
        [SerializeField]
        private Collider2D m_swordUpSlashAttackCollider;

        private void Start()
        {
            attackCollider.enabled = false;
            crouchState = GetComponent<Crouch>();
            dashState = GetComponent<Dash>();

        }

        // Update is called once per frame
        void Update()
        {
            var canSlash = inputState.GetButtonValue(inputButtons[0]);
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
            var downButton = inputState.GetButtonValue(inputButtons[1]);
            var upButton = inputState.GetButtonValue(inputButtons[2]);


            //if(canSlash && attackCounter <= comboCount)
            //{
            //    Debug.Log(startTimeBtwAttck);
            //    if(startTimeBtwAttck > 0.1f)
            //    {
            //        Debug.Log("attacking:" + attackCounter);
            //        attackCounter++;
            //        timeBtwnAtck -= Time.deltaTime;
            //    }
            //    else
            //    {
            //        attackCounter = 0;
            //        timeBtwnAtck = resetTime;
            //    }

            //}

            //if (attackTimeCounter > 0.1f)
            //{
            //    attackTimeCounter -= Time.deltaTime;
            //    //attacking = true;
            //    if (canSlash && holdTime < 0.1f)
            //    {
            //        attackCounter++;
            //    }
            //    Debug.Log("attack:" + attackCounter);
            //}
            //else
            //{
            //    attackTimeCounter = resetTime;
            //    attackCounter = 0;
            //}

            upHold = upButton ? true : false;

            if (timeBtwnAtck < 0 && !dashState.dashing)
            {
                if (canSlash && holdTime < 0.1f && attacking == false)
                {
                    ToggleScripts(false);
                    if (collisionState.grounded)
                    {
                        if (attackCounter == 0)
                        {
                            comboTimer = attackingTime;
                        }
                        else if (attackCounter == 3)
                        {
                            attackCounter = 0;
                            comboTimer = 1.0f;
                        }
                        else
                        {
                            //nothing yet
                        }

                        //Collider2D[] objToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                        if (attackCounter == 0 && !upButton && !downButton)
                        {
                            m_forwardSlash1FX.Play();
                            m_forwardSlashAttackCollider.enabled = true;
                        }
                        else if (attackCounter == 1 && !upButton && !downButton)
                        {
                            m_swordCombo1FX.Play();
                            m_swordCombo1AttackCollider.enabled = true;
                        }
                        else if (attackCounter == 2 && !upButton && !downButton)
                        {
                            m_swordCombo2FX.Play();
                            m_swordCombo2AttackCollider.enabled = true;
                        }
                    }

                    attacking = true;
                }

                timeBtwnAtck = startTimeBtwAttck;
            }
            else
            {
                timeBtwnAtck -= Time.deltaTime;
            }
            if (comboTimer > 0.1f && attackCounter >= 1)
            {
                comboTimer -= Time.deltaTime;
            }
            else
            {
                comboTimer = attackingTime;
                attackCounter = 0;
            }

            if (!collisionState.grounded && holdTime < 0.3f)
            {

                holdingAttack = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }

        private void CrouchSlashFX()
        {
            m_VFX_CrouchSlashX.Play();
            m_crouchSlashAttackCollider.enabled = true;
        }

        private void SwordAttackForward_MainAction()
        {
            m_forwardSlash1FX.Play();
        }

        private void JumpDownSlashFX()
        {
            m_VFX_JumpSwordDownSlashFX.Play();
        }

        private void SwordJumpSlashForwardFX()
        {
            m_VFX_SwordJumpSlashForward.Play();
        }

        private void JumpUpSlashFX()
        {
            m_VFX_JumpUpSlashFX.Play();
            m_jumpSlashAttackCollider.enabled = true;
        }

        private void SwordUpSlashFX()
        {
            m_VFX_SwordUpSlashFX.Play();
            m_swordUpSlashAttackCollider.enabled = true;
        }

        private void FinishAttackAnim()
        {
            attackCollider.enabled = false;

            switch (attackCounter)
            {
                case 0:
                    m_forwardSlashAttackCollider.enabled = false;
                    break;
                case 1:
                    m_swordCombo1AttackCollider.enabled = false;
                    break;
                case 2:
                    m_swordCombo2AttackCollider.enabled = false;
                    break;
                default:
                    break;
            }

            m_crouchSlashAttackCollider.enabled = false;
            m_jumpSlashAttackCollider.enabled = false;
            m_swordUpSlashAttackCollider.enabled = false;

            attackCounter++;
            attacking = false;
            ToggleScripts(true);

        }
    }
}

