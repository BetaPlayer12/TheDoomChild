using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Slash : PlayerBehaviour
    {

        private Animator animator;
        public float timeBtwnAtck = 0.2f;
        private float attackTimeCounter;
        public float attackingTime;
        //public int attackCounter;
        private float attackHold;
        private float comboTimer;
        private float slashTimer;
        private Dash dashState;
        public int comboCount = 0;
        //public float startTimeBtwAttck;
        public bool attackHolding;
        public bool attacking;
        public bool slashing;
        public bool upHold;
        // public float attackHold = 0.5f;
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
        private Collider2D m_swordJumpSlashForwardAttackCollider;
        [SerializeField]
        private Collider2D m_swordUpSlashAttackCollider;

        public int numOfClicks = 0;
        float lastClickedTime = 0;
        public float maxComboDelay = 1.2f;

        private void Start()
        {
            attackCollider.enabled = false;
            crouchState = GetComponent<Crouch>();
            dashState = GetComponent<Dash>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            //var canSlash = inputState.GetButtonValue(inputButtons[0]);
            var canSlash = Input.GetButtonDown("Fire1");
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
            var downButton = inputState.GetButtonValue(inputButtons[1]);
            var upButton = inputState.GetButtonValue(inputButtons[2]);
            var leftButton = inputState.GetButtonValue(inputButtons[3]);
            var rightButton = inputState.GetButtonValue(inputButtons[4]);
            


            if (Time.time - lastClickedTime > maxComboDelay)
            {
                numOfClicks = 0;
            }

            upHold = upButton ? true : false;
            animator.SetBool("UpHold", upHold);

            if (Input.GetButtonDown("Fire1") && !dashState.dashing)
            {
                ToggleScripts(false);
               
                if (!upHold && collisionState.grounded && !downButton)
                {
                    
                    lastClickedTime = Time.time;
                    numOfClicks++;

                    if (leftButton || rightButton && collisionState.grounded)
                    {
                       
                        body2d.velocity = Vector2.zero;
                        animator.SetBool("Attack", false);
                        numOfClicks = 1;
                       
                    }

                    if (numOfClicks == 1)
                    {
                        animator.SetBool("Slash1", true);
                        m_forwardSlash1FX.Play();
                    }
                    numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);
                   
                    switch (numOfClicks)
                    {
                        case 1:
                            animator.SetBool("Slash1", true);
                            animator.SetBool("Slash2", false);
                            animator.SetBool("Slash3", false);
                            break;
                        case 2:
                            animator.SetBool("Slash1", false);
                            animator.SetBool("Slash2", true);
                            animator.SetBool("Slash3", false);
                            m_swordCombo1FX.Play();
                            break;
                        case 3:
                            animator.SetBool("Slash1", false);
                            animator.SetBool("Slash2", false);
                            animator.SetBool("Slash3", true);
                            m_swordCombo2FX.Play();
                            break;
                    }
                    animator.SetBool("Attack", true);
                }else if(upHold && collisionState.grounded)
                {
                    animator.SetBool("Attack", true);
                    SwordUpSlashFX();
                }else if(downButton && collisionState.grounded)
                {

                    Debug.Log("Crouch attack");
                    animator.SetBool("Attack", true);
                    animator.SetBool("Crouch", true);
                    CrouchSlashFX();
                }else if (!collisionState.grounded)
                {
                    if (!upButton)
                    {
                       
                        SwordJumpSlashForwardFX();

                    }
                    else
                    {
                        JumpUpSlashFX();
                    }
                    animator.SetBool("Attack", true);
                }
                else
                {
                   
                }
                
            }else if (Input.GetButtonUp("Fire1"))
            {
                ToggleScripts(true);
            }
        }

        public void ReturnToIdle()
        {
            //force set to Idle
            Debug.Log("hahahahahahah");
            animator.SetInteger("Jog", 0);
            animator.SetBool("Grounded", true);
            animator.SetBool("Crouch", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Dash", false);
            animator.SetBool("EarthShake", false);
            animator.SetBool("ThrustCharge", false);
            animator.SetBool("Thrust", false);
            animator.SetBool("JumpDownAttack", false);
            animator.SetBool("WallGrab", false);
            animator.SetBool("Slash1", false);
            animator.SetBool("Slash2", false);
            animator.SetBool("Slash3", false);
            //animator.SetBool()

        }

        public void FinishAttack1()
        {
            Debug.Log("attack 1 only " + numOfClicks);
            animator.SetBool("Slash1", false);
            animator.SetBool("Attack", false);
            ToggleScripts(true);
        }

        public void FinishAttack2()
        {
           
                Debug.Log("attack 2 only " + numOfClicks);
                animator.SetBool("Slash1", false);
                animator.SetBool("Slash2", false);
                animator.SetBool("Attack", false);
                ToggleScripts(true);
           
        }

        public void FinishAttack3()
        {
            Debug.Log("attack 3 only " + numOfClicks);
            animator.SetBool("Slash1", false);
            animator.SetBool("Slash2", false);
            animator.SetBool("Slash3", false);
            animator.SetBool("Attack", false);
            numOfClicks = 0;
            ToggleScripts(true);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }

        private void CrouchSlashFX()
        {
            Debug.Log("Crounch attack");
            m_VFX_CrouchSlashX.Play();
            m_crouchSlashAttackCollider.enabled = true;
        }

        private void SwordAttackForward_MainAction()
        {
            Debug.Log("forward slash");
            m_forwardSlash1FX.Play();
        }

        private void JumpDownSlashFX()
        {
            m_VFX_JumpSwordDownSlashFX.Play();
        }

        private void SwordJumpSlashForwardFX()
        {
            m_VFX_SwordJumpSlashForward.Play();
            m_swordJumpSlashForwardAttackCollider.enabled = true;
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

            Debug.Log("finish attack");
            attackCollider.enabled = false;

            m_forwardSlashAttackCollider.enabled = false;
            m_swordCombo1AttackCollider.enabled = false;
            m_swordCombo2AttackCollider.enabled = false;

            m_crouchSlashAttackCollider.enabled = false;
            m_jumpSlashAttackCollider.enabled = false;
            m_swordUpSlashAttackCollider.enabled = false;
            m_swordJumpSlashForwardAttackCollider.enabled = false;

            animator.SetBool("Slash1", false);
            animator.SetBool("Slash2", false);
            animator.SetBool("Slash3", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Crouch", false);

            attacking = false;
            slashing = false;
            ToggleScripts(true);
        }
    }
}

