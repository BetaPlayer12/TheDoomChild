using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Slash : PlayerBehaviour
    {
        [SerializeField]
        private int m_maxComboNumber = 0;
        [SerializeField]
        private float m_attackDelay = 0;
        [SerializeField]
        private float m_comboDelay = 1.5f;
        [SerializeField]
        private float m_airAttackDelay;

        private bool canAttack = true;
        private int slashStateIndex = 0;
        private float attackDelay = 0;
        public int currentSlashState;

        private Animator animator;
        public float timeBtwnAtck = 0.2f;
        private float attackTimeCounter;
        private float releaseTime = 0.0f;

        public float attackingTime;
        //public int attackCounter;
        private float attackHold;
        private float comboTimer;
        private float slashTimer;
        private Dash dashState;
        private GroundShaker groundShaker;
        public int comboCount = 0;
        //public float startTimeBtwAttck;
        public bool attackHolding = false;
        public bool attacking;
        public bool slashing;
        public bool upHold;
        public bool downHold;
        // public float attackHold = 0.5f;
        public float resetTime;

        public Transform attackPos;
        public Collider2D attackCollider;
        public LayerMask whatIsEnemies;

        public float attackRange;
        public bool holdingAttack;

        private Dock crouchState;

        [SerializeField]
        private CollisionRegistrator m_collisionRegistrator;

        //FX
        [SerializeField, Header("FX")]
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

        //Colliders
        [SerializeField, Header("Colliders")]
        private List<Collider2D> m_swordSlashColliders;
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
        [SerializeField]
        private Collider2D m_swordDownSlashAttackCollider;

        public int numOfClicks = 0;
        float lastClickedTime = 0;
        public float maxComboDelay = 1.2f;

        [SerializeField, Header("Damage Stuff"), MinValue(0)]
        private float m_slash1DamageModifier;
        [SerializeField]
        private float m_slash2DamageModifier;
        [SerializeField]
        private float m_slash3DamageModifier;
        [SerializeField]
        private List<float> m_slashModifierList;

        PlayerMovement playerMovement;

        private void Start()
        {
            attackCollider.enabled = false;
            crouchState = GetComponent<Dock>();
            dashState = GetComponent<Dash>();
            groundShaker = GetComponent<GroundShaker>();
            animator = GetComponent<Animator>();

            playerMovement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            if (attackDelay <= 0)
            {
                if (inputState.slashPressed && !stateManager.isAttacking && stateManager.isDead == false)
                {
                    stateManager.isAttacking = true;
                    stateManager.isInCombatMode = true;
                    stateManager.isIdle = false;

                    if (stateManager.isGrounded)
                    {
                        playerMovement.DisableMovement();
                        currentSlashState = slashStateIndex;
                        attacker.SetDamageModifier(m_slashModifierList[currentSlashState]);
                        slashStateIndex++;

                        StartCoroutine(SlashDelayRoutine(currentSlashState));

                        if (slashStateIndex >= m_maxComboNumber)
                        {
                            slashStateIndex = 0;
                        }

                        attackDelay = m_attackDelay * m_comboDelay;
                    }
                    else
                    {
                        attacker.SetDamageModifier(1);

                        StartCoroutine(MidairSlashDelayRoutine());
                    }
                }
                else if(stateManager.isAttacking && inputState.slashPressed)
                {
                    Debug.Log("SLashing");
                }
            }
            else if (attackDelay > 0)
            {
                attackDelay -= Time.deltaTime;
            }

            if(stateManager.isFlinching)
            {
                
            }

            //var canSlash = Input.GetButtonDown("Fire1");
            //var downButton = inputState.GetButtonValue(inputButtons[1]);
            //var upButton = inputState.GetButtonValue(inputButtons[2]);
            //var leftButton = inputState.GetButtonValue(inputButtons[3]);
            //var rightButton = inputState.GetButtonValue(inputButtons[4]);

            //if (Time.time - lastClickedTime > maxComboDelay)
            //{
            //    numOfClicks = 0;
            //}

            //upHold = upButton ? true : false;
            //animator.SetBool("UpHold", upHold);

            //downHold = downButton ? true : false;
            //animator.SetBool("DownHold", downHold);

            //if (!collisionState.grounded)
            //{
            //    if (Input.GetButtonDown("Fire1"))
            //    {
            //        attacking = true;

            //        releaseTime += Time.deltaTime;
            //    }
            //    if (Input.GetButtonUp("Fire1"))
            //    {
            //        if (upButton)
            //        {
            //            animator.SetBool("Attack", true);
            //            JumpUpSlashFX();
            //            m_swordUpSlashAttackCollider.enabled = true;
            //        }
            //        if (downButton)
            //        {
            //            if (releaseTime < 0.15f)
            //            {
            //                animator.SetBool("Attack", true);
            //                JumpDownSlashFX();
            //                Debug.Log("down attack");
            //            }
            //        }
            //        else
            //        {
            //            animator.SetBool("Attack", true);
            //            m_swordJumpSlashForwardAttackCollider.enabled = true;
            //        }

            //        attacking = true;
            //        releaseTime = 0.0f;
            //    }
            //}

            //if (canSlash && !dashState.dashing)
            //{
            //    m_collisionRegistrator.ResetHitCache();
            //    ToggleScripts(false);
            //    attacking = true;

            //    if (!upHold && collisionState.grounded && !downButton)
            //    {
            //        lastClickedTime = Time.time;
            //        numOfClicks++;

            //        if (leftButton || rightButton && collisionState.grounded)
            //        {
            //            body2d.velocity = Vector2.zero;
            //            animator.SetBool("Attack", false);
            //            numOfClicks = 1;
            //        }

            //        if (numOfClicks == 1)
            //        {
            //            animator.SetBool("Slash1", true);
            //            attacker.SetDamageModifier(m_slash1DamageModifier);
            //            m_forwardSlashAttackCollider.enabled = true;
            //        }

            //        numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);

            //        if (numOfClicks > 3)
            //        {
            //            numOfClicks = 1;
            //        }

            //        animator.SetBool("Attack", true);

            //        switch (numOfClicks)
            //        {
            //            case 1:
            //                animator.SetBool("Slash1", true);
            //                animator.SetBool("Slash2", false);
            //                animator.SetBool("Slash3", false);
            //                //VFX_Attack1();
            //                attacker.SetDamageModifier(m_slash1DamageModifier);
            //                m_forwardSlashAttackCollider.enabled = true;
            //                break;
            //            case 2:
            //                animator.SetBool("Slash1", false);
            //                animator.SetBool("Slash2", true);
            //                animator.SetBool("Slash3", false);
            //                //VFX_Attack2();
            //                attacker.SetDamageModifier(m_slash2DamageModifier);
            //                m_swordCombo1AttackCollider.enabled = true;
            //                break;
            //            case 3:
            //                animator.SetBool("Slash1", false);
            //                animator.SetBool("Slash2", false);
            //                animator.SetBool("Slash3", true);
            //                //VFX_Attack3();
            //                attacker.SetDamageModifier(m_slash3DamageModifier);
            //                m_swordCombo2AttackCollider.enabled = true;
            //                break;
            //        }
            //    }
            //    else if (upHold && collisionState.grounded)
            //    {
            //        animator.SetBool("Attack", true);
            //        SwordUpSlashFX();
            //    }
            //    else if (downButton && collisionState.grounded)
            //    {
            //        animator.SetBool("Attack", true);
            //        animator.SetBool("Crouch", true);
            //        CrouchSlashFX();
            //    }
            //}
            //else if (Input.GetButtonUp("Fire1") && collisionState.grounded)
            //{
            //    m_forwardSlashAttackCollider.enabled = false;
            //    m_swordCombo1AttackCollider.enabled = false;
            //    m_swordCombo2AttackCollider.enabled = false;
            //    m_crouchSlashAttackCollider.enabled = false;
            //    m_jumpSlashAttackCollider.enabled = false;
            //    m_swordUpSlashAttackCollider.enabled = false;
            //    m_swordJumpSlashForwardAttackCollider.enabled = false;
            //    attacking = false;
            //    animator.SetBool("Attack", false);
            //    animator.SetBool("Slash1", false);
            //    ToggleScripts(true);
            //}
        }

        public void ShowAttackCollider()
        {
            m_collisionRegistrator.ResetHitCache();
            m_swordSlashColliders[currentSlashState].enabled = true;
        }

        public void SlashAnimationFinished()
        {
            if (inputState.slashPressed)
            {
                Debug.Log("INT");
            }

            stateManager.isAttacking = false;
            stateManager.isInCombatMode = true;
            stateManager.isIdle = true;

            for (int i = 0; i < m_swordSlashColliders.Count; i++)
            {
                m_swordSlashColliders[i].enabled = false;
            }

            playerMovement.EnableMovement();
        }

        private IEnumerator SlashDelayRoutine(int index)
        {
            yield return new WaitForSeconds(m_attackDelay);
            m_swordSlashColliders[index].enabled = false;

            SlashAnimationFinished();
        }

        private IEnumerator MidairSlashDelayRoutine()
        {
            yield return new WaitForSeconds(m_airAttackDelay);

            MidairSlashAnimationFinished();
        }

        public void MidairSlashAnimationFinished()
        {
            stateManager.isAttacking = false;
            stateManager.isInCombatMode = true;
            stateManager.isIdle = true;
        }

        public void ReturnToIdle()
        {
            //force set to Idle
            animator.SetInteger("Jog", 0);
            animator.SetBool("Grounded", true);
            animator.SetBool("Crouch", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Dash", false);
            animator.SetBool("EarthShake", false);
            animator.SetBool("ThrustCharge", false);
            //animator.SetBool("Thrust", false);
            animator.SetBool("JumpDownAttack", false);
            animator.SetBool("WallGrab", false);
            animator.SetBool("Slash1", false);
            animator.SetBool("Slash2", false);
            animator.SetBool("Slash3", false);
        }

        public void FinishCrouchAttack()
        {
            Debug.Log("Finish crouch attack");
        }

        public void FinishAttack1()
        {
            stateManager.isAttacking = false;
            stateManager.isInCombatMode = true;

            //animator.SetBool("Slash1", false);
            //animator.SetBool("Attack", false);
            //m_forwardSlashAttackCollider.enabled = false;
            //m_swordCombo1AttackCollider.enabled = false;
            //m_swordCombo2AttackCollider.enabled = false;

            //m_crouchSlashAttackCollider.enabled = false;
            //m_jumpSlashAttackCollider.enabled = false;
            //m_swordUpSlashAttackCollider.enabled = false;
            //m_swordJumpSlashForwardAttackCollider.enabled = false;
            //ToggleScripts(true);
            //attacking = false;
        }

        public void FinishAttack2()
        {
            animator.SetBool("Slash1", false);
            animator.SetBool("Slash2", false);
            animator.SetBool("Attack", false);
            m_forwardSlashAttackCollider.enabled = false;
            m_swordCombo1AttackCollider.enabled = false;
            m_swordCombo2AttackCollider.enabled = false;

            m_crouchSlashAttackCollider.enabled = false;
            m_jumpSlashAttackCollider.enabled = false;
            m_swordUpSlashAttackCollider.enabled = false;
            m_swordJumpSlashForwardAttackCollider.enabled = false;
            ToggleScripts(true);
            attacking = false;
        }

        public void FinishAttack3()
        {
            animator.SetBool("Slash1", false);
            animator.SetBool("Slash2", false);
            animator.SetBool("Slash3", false);
            animator.SetBool("Attack", false);
            m_forwardSlashAttackCollider.enabled = false;
            m_swordCombo1AttackCollider.enabled = false;
            m_swordCombo2AttackCollider.enabled = false;

            m_crouchSlashAttackCollider.enabled = false;
            m_jumpSlashAttackCollider.enabled = false;
            m_swordUpSlashAttackCollider.enabled = false;
            m_swordJumpSlashForwardAttackCollider.enabled = false;
            numOfClicks = 0;
            ToggleScripts(true);
            attacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }

        private void VFX_Attack1()
        {
            m_forwardSlash1FX.Play();
        }

        private void VFX_Attack2()
        {
            m_swordCombo1FX.Play();
        }

        private void VFX_Attack3()
        {
            m_swordCombo2FX.Play();
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
            m_collisionRegistrator.ResetHitCache();
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

            //ToggleScripts(true);
        }

        public void Test(int dudalala)
        {

        }
    }
}

//void Update()
//{
//    var canSlash = Input.GetButtonDown("Fire1");
//    var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
//    var downButton = inputState.GetButtonValue(inputButtons[1]);
//    var upButton = inputState.GetButtonValue(inputButtons[2]);
//    var leftButton = inputState.GetButtonValue(inputButtons[3]);
//    var rightButton = inputState.GetButtonValue(inputButtons[4]);


//    if (canSlash && !attacking)
//    {
//        attacking = true;
//        numOfClicks++;
//    }

//    if (attacking)
//    {
//        attackingTime += Time.deltaTime;
//        if (canSlash)
//        {
//            //max time between attack
//            if (collisionState.grounded)
//            {
//                numOfClicks++;
//            }
//            else
//            {
//                ResetAttacking();
//            }
//        }

//        if (attackingTime > 0.6f)
//        {

//            ResetAttacking();
//        }

//    }


//    Debug.Log("attacking time: " + attackingTime + " numclicks: " + numOfClicks);
//}