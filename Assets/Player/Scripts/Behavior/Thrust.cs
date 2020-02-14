using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Thrust : PlayerBehaviour
    {
        public bool chargingAttack;
        public bool thrustAttack;
        public bool thrustHasStarted;
        public float timeToCharge;
        [SerializeField]
        private ParticleSystem swordThrustBuildUp;
        [SerializeField]
        private ParticleSystem swordThrustBody;
        [SerializeField]
        private ParticleSystem slashSwordThrustImpacts;
        [SerializeField]
        private Collider2D m_thrustImpactAttackCollider;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);
            var down = inputState.GetButtonValue(inputButtons[1]);
            var dash = inputState.GetButtonValue(inputButtons[2]);

            // Debug.Log(holdTime + " holding");
            if (holdTime > timeToCharge && !chargingAttack && collisionState.grounded && !down && !dash)
            {
                ToggleScripts(false);
                Debug.Log("Charging");
                swordThrustBuildUp.Play();
                thrustAttack = true;
               

            }
            else if (chargingAttack && holdTime == 0)
            {
                swordThrustBuildUp.Stop();
                swordThrustBody.Play();
                Debug.Log("Attack");
                chargingAttack = false;
            }
        }

        private void StartChargeLoop()
        {
            
            Debug.Log("charge start ");
            chargingAttack = true;
            thrustHasStarted = true;
           
        }

        private void ThrustImpact()
        {
            slashSwordThrustImpacts.Play();
            m_thrustImpactAttackCollider.enabled = true;
        }

        private void FinishThrustAttackAnime()
        {
            
            thrustAttack = false;
            thrustHasStarted = false;
            m_thrustImpactAttackCollider.enabled = false;
            ToggleScripts(true);
        }
    }

}
