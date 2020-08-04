using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class GroundShaker : PlayerBehaviour
    {
        [SerializeField]
        private ParticleSystem deathEarthShakerHelicopter;
        [SerializeField]
        private ParticleSystem deathEarthShakerPreloop;
        [SerializeField]
        private ParticleSystem deathEarthShakerLoop;
        [SerializeField]
        private ParticleSystem deathEarthShakerImpact;
        [SerializeField]
        private Collider2D m_groundShakerAttackCollider;
        [SerializeField]
        private WallSlide wallSlide;

        public float midAirDelay;
        public float midAirAttackHold;
        public bool groundSmash;
        public float smashMultiplier;
        private float defGravity;
        private Animator animator;

        [SerializeField, Header("Damage Stuff"), MinValue(0)]
        private float m_damageModifier;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Start Ground Shaker");
            defGravity = rigidBody.gravityScale;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            var down = inputState.GetButtonValue(inputButtons[0]);
            var attack = inputState.GetButtonValue(inputButtons[1]);
            var attackHold = inputState.GetButtonHoldTime(inputButtons[1]);

            if (!stateManager.isGrounded && down && attack && !groundSmash && attackHold > midAirAttackHold && !stateManager.onWall)
            {
                playerMovement.DisableMovement();
                rigidBody.velocity = Vector2.zero;
                groundSmash = true;
                rigidBody.gravityScale = 0f;
                ToggleScripts(false);
                StopAllCoroutines();
                StartCoroutine(GroundSmashDelayRoutine());
            }
        }

        private void StartEarthShakerFX()
        {
            deathEarthShakerHelicopter.Play();
        }

        private void DeathEarthShakerPreLoop()
        {
            deathEarthShakerHelicopter.Stop();
            deathEarthShakerPreloop.Play();
        }
        private void DeathEarthShakerLoop()
        {
            deathEarthShakerPreloop.Stop();
            deathEarthShakerLoop.Play();
        }

        private void DeathEarthShakerImpact()
        {
            rigidBody.velocity = Vector2.zero;
            deathEarthShakerLoop.Stop();
            deathEarthShakerImpact.Play();
            attacker.SetDamageModifier(m_damageModifier);
            m_groundShakerAttackCollider.enabled = true;
        }

        public void GroundSmashFinishAnimation()
        {
            groundSmash = false;
            rigidBody.velocity = Vector2.zero;

            ToggleScripts(true);
            m_groundShakerAttackCollider.enabled = false;
            animator.SetBool("Attack", false);
            playerMovement.EnableMovement();
        }

        IEnumerator GroundSmashDelayRoutine()
        {
            yield return new WaitForSeconds(midAirDelay);
            rigidBody.gravityScale = defGravity;
            // body2d.gravityScale = defGravity * smashMultiplier;
            Debug.Log(smashMultiplier);
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(new Vector2(rigidBody.velocity.x, -smashMultiplier), ForceMode2D.Force);
        }
    }
}
