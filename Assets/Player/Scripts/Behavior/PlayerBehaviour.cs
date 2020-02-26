using Sirenix.OdinInspector;
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
        protected InputState inputState;
        protected Rigidbody2D body2d;
        protected Transform trans;
        protected CollisionState collisionState;
        protected CapsuleCollider2D capsuleCollider;

        protected virtual void Awake()
        {
            inputState = GetComponent<InputState>();
            body2d = GetComponentInParent<Rigidbody2D>();
            trans = GetComponent<Transform>();
            collisionState = GetComponent<CollisionState>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void ToggleScripts(bool value)
        {
            foreach (var script in dissableScripts)
            {
                script.enabled = value;
            }
        }
    }

}