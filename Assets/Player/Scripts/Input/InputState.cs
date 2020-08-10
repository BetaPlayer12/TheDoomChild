using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holysoft.Event;

namespace PlayerNew
{
    public class ButtonState
    {
        public bool value;
        public float holdTime = 0;
    }

    public enum Directions
    {
        Right = 1,
        Left = -1
    }

    [DefaultExecutionOrder(-100)]
    public class InputState : MonoBehaviour
    {
        public Directions direction = Directions.Right;
        public float absValX = 0f;
        public float absValY = 0f;

        private Rigidbody2D body2D;
        private StateManager collisionState;
        private Dictionary<Buttons, ButtonState> buttonStates = new Dictionary<Buttons, ButtonState>();

        [SerializeField]
        private InputManager m_inputManager;
        [HideInInspector]
        public float horizontal;
        [HideInInspector]
        public float vertical;
        [HideInInspector]
        public bool upHeld;
        [HideInInspector]
        public bool downHeld;
        [HideInInspector]
        public bool dashPressed;
        [HideInInspector]
        public bool slashPressed;
        [HideInInspector]
        public bool slashHeld;
        [HideInInspector]
        public bool levitatePressed;
        [HideInInspector]
        public bool levitateHeld;
        [HideInInspector]
        public bool whipAttack;
        [HideInInspector]
        public bool whipJumpAttack;

        bool readyToClear;
        private void OnControllerEnabled(object sender, EventActionArgs eventArgs)
        {
            enabled = true;
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            readyToClear = true;
            ClearInput();
            enabled = false;
        }

        private void Awake()
        {
            body2D = GetComponentInParent<Rigidbody2D>();
            collisionState = GetComponent<StateManager>();
            m_inputManager.ControllerDisabled += OnControllerDisabled;
            m_inputManager.ControllerEnabled += OnControllerEnabled;
        }

        private void Update()
        {
            ClearInput();
            ProcessInputs();
            horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        }

        private void FixedUpdate()
        {
            readyToClear = true;

            //absValX = Mathf.Abs(body2D.velocity.x);

            //if (absValX > 0 && absValX < 1)
            //{
            //    absValX = 0;
            //}

            //absValY = Mathf.Abs(body2D.velocity.y);

            //if (absValY > 0 && absValY < 1)
            //{
            //    absValY = 0;
            //}
        }

        void ClearInput()
        {
            //If we're not ready to clear input, exit
            if (!readyToClear)
                return;

            //Reset all inputs
            horizontal = 0f;
            vertical = 0f;
            //dashPressed = false;
            slashPressed = false;
            slashHeld = false;
            levitatePressed = false;
            levitateHeld = false;
            whipAttack = false;
            whipJumpAttack = false;

            readyToClear = false;
        }

        private void ProcessInputs()
        {
            //Movement Action Buttons
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            //upHeld = Input.GetButton("Up");
            //downHeld = Input.GetButton("Down");
            dashPressed = Input.GetButtonDown("Dash");

            //Offensive Action Buttons
            slashPressed = Input.GetButtonDown("Fire1");
            slashHeld = Input.GetButton("Fire1");
            levitatePressed = Input.GetButtonDown("Levitate");
            levitateHeld = Input.GetButton("Levitate");
            whipAttack = Input.GetButtonDown("Fire2");
            whipJumpAttack = Input.GetButtonDown("Fire2");
        }

        public void SetButtonValue(Buttons key, bool value)
        {
            if (!buttonStates.ContainsKey(key))
                buttonStates.Add(key, new ButtonState());

            var state = buttonStates[key];

            if (state.value && !value)
            {
                state.holdTime = 0;
            }
            else if (state.value && value)
            {
                state.holdTime += Time.deltaTime;
            }

            state.value = value;
        }

        public bool GetButtonValue(Buttons key)
        {
            if (buttonStates.ContainsKey(key))
                return buttonStates[key].value;
            else
                return false;
        }

        public float GetButtonHoldTime(Buttons key)
        {
            if (buttonStates.ContainsKey(key))
                return buttonStates[key].holdTime;
            else
                return 0;
        }
    }
}