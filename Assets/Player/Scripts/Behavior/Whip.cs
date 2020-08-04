using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public class Whip : PlayerBehaviour
    {
        [SerializeField]
        private Collider2D whipCollider;
        [SerializeField]
        private float m_whipAttackCooldown;
        [SerializeField, Header("Damage Stuff"), MinValue(0)]
        private float m_damageModifier;

        private StateManager coll;

        public bool whipAtk = false;
        public bool whipJumpAttack = false;

        private void Start()
        {
            coll = GetComponent<StateManager>();
        }

        void Update()
        {
            var canWhip = inputState.GetButtonValue(inputButtons[0]);

            if (inputState.whipAttack)
            {
                whipAtk = true;
                ToggleScripts(false);
            }
            if (inputState.whipJumpAttack)
            {
                whipJumpAttack = true;
                ToggleScripts(false);
            }

            //if (canWhip && !whipAtk)
            //{
            //    whipAtk = true;
            //    ToggleScripts(false);
            //}
            //if(jumpWhipAttack)
            //{

            //}
        }

        private void WhipColliderEnable()
        {
            attacker.SetDamageModifier(m_damageModifier);
            whipCollider.enabled = true;
        }

        private void WhipFinishAttack()
        {
            Debug.Log(coll.isGrounded);
            whipCollider.enabled = false;
            whipAtk = false;
            whipJumpAttack = false;

            ToggleScripts(true);
            StartCoroutine("WhipDelay");
        }

        private IEnumerator WhipDelay()
        {
            yield return new WaitForSeconds(m_whipAttackCooldown);
        }
    }
}

