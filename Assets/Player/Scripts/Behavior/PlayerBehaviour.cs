﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        public Buttons[] inputButtons;
        public MonoBehaviour[] dissableScripts;
        protected InputState inputState;
        protected Rigidbody2D body2d;
        protected Transform trans;
        protected CollisionState collisionState;

        protected virtual void Awake()
        {
            inputState = GetComponent<InputState>();
            body2d = GetComponent<Rigidbody2D>();
            trans = GetComponent<Transform>();
            collisionState = GetComponent<CollisionState>();
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