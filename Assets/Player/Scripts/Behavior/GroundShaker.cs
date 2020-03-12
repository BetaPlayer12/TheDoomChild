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

        public float midAirDelay;
        public bool groundSmash;
        public float smashMultiplier;
        private float defGravity;
        private Animator animator;
        // Start is called before the first frame update
        void Start()
        {
            defGravity = body2d.gravityScale;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            var down = inputState.GetButtonValue(inputButtons[0]);
            var attack = inputState.GetButtonValue(inputButtons[1]);
            var attackHold = inputState.GetButtonHoldTime(inputButtons[1]);



            if (!collisionState.grounded && down && attack && !groundSmash)
            {
              

                body2d.velocity = Vector2.zero;
                groundSmash = true;
                body2d.gravityScale = 0f;
                ToggleScripts(false);
                StartCoroutine(GroundSmashDelayRoutine());
            }
            else
            {
                // Debug.Log("grounded");
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
            deathEarthShakerLoop.Stop();
            deathEarthShakerImpact.Play();
            m_groundShakerAttackCollider.enabled = true;
            
        }

        IEnumerator GroundSmashDelayRoutine()
        {
            yield return new WaitForSeconds(midAirDelay);
            // body2d.gravityScale = defGravity * smashMultiplier;
            body2d.velocity = Vector2.zero;
            body2d.AddForce(new Vector2(body2d.velocity.x, -smashMultiplier), ForceMode2D.Force);
        }


        public void GroundSmashFinishAnimation()
        {
            groundSmash = false;
            body2d.velocity = Vector2.zero;
            body2d.gravityScale = defGravity;
            ToggleScripts(true);
            m_groundShakerAttackCollider.enabled = false;
            animator.SetBool("Attack", false);
        }
    }
}
