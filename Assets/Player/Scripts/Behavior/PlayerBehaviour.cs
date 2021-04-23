using Sirenix.OdinInspector;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay;

namespace PlayerNew
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        [DrawWithUnity]
        public Buttons[] inputButtons;
        [DrawWithUnity]
        public MonoBehaviour[] dissableScripts;
        [DrawWithUnity]
        public MonoBehaviour[] enableOnlyScript;
        protected InputState inputState;
        protected Rigidbody2D rigidBody;
        protected Transform trans;
        protected StateManager stateManager;
        protected CapsuleCollider2D capsuleCollider;
        protected Damageable damagable;
        protected BasicHealth basicHealth;
        protected Attacker attacker;
        protected Animator animator;
        protected FaceDirection facing;
        protected Character character;
        protected PlayerMovement playerMovement;
        private bool m_isDoingAnimation = false;

        protected virtual void Awake()
        {
            inputState = GetComponent<InputState>();
            rigidBody = GetComponentInParent<Rigidbody2D>();
            trans = GetComponent<Transform>();
            stateManager = GetComponent<StateManager>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
            damagable = GetComponentInParent<Damageable>();
            basicHealth = GetComponentInChildren<BasicHealth>();
            attacker = GetComponentInParent<Attacker>();
            animator = GetComponent<Animator>();
            facing = GetComponent<FaceDirection>();
            character = GetComponentInParent<Character>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        protected virtual void ToggleScripts(bool value)
        {             
            if(enableOnlyScript.Length > 0 && value == true)
            {
                foreach (var enbleScript in enableOnlyScript)
                {
                    Debug.Log("enable only:" + enbleScript);
                    if (!enbleScript)
                    {
                        foreach (var script in dissableScripts)
                        {
                            script.enabled = value;
                        }
                    }
                }
            }
            else
            {
                foreach (var script in dissableScripts)
                {
                    script.enabled = value;
                }
            }
        }
    }
}