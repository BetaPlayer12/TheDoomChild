﻿using Sirenix.OdinInspector;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        protected Rigidbody2D body2d;
        protected Transform trans;
        protected CollisionState collisionState;
        protected CapsuleCollider2D capsuleCollider;
        protected Damageable damagable;
        protected BasicHealth basicHealth;
        protected Attacker attacker;

        protected virtual void Awake()
        {
            inputState = GetComponent<InputState>();
            body2d = GetComponentInParent<Rigidbody2D>();
            trans = GetComponent<Transform>();
            collisionState = GetComponent<CollisionState>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
            damagable = GetComponentInParent<Damageable>();
            basicHealth = GetComponentInChildren<BasicHealth>();
            attacker = GetComponentInParent<Attacker>();
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