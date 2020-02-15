using UnityEngine;
using System.Collections;
using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;

namespace PlayerNew
{
    public enum Buttons
    {
        Right,
        Left,
        Up,
        Down,
        Jump,
        Attack,
        Dash
    }

    public enum Condition
    {
        GreaterThan,
        LessThan
    }

    [System.Serializable]
    public class InputAxisState
    {
        public string axisName;
        public float offValue;
        public Buttons button;
        public Condition condition;

        public bool value
        {

            get
            {
                var val = Input.GetAxisRaw(axisName);
                //var val = Input.GetAxis(axisName);

                switch (condition)
                {
                    case Condition.GreaterThan:
                        return val > offValue;
                    case Condition.LessThan:
                        return val < offValue;
                }

                return false;
            }

        }
    }

    public class InputManager : MonoBehaviour,  IMainController
    {

        public InputAxisState[] inputs;
        public InputState inputState;

        public event EventAction<EventActionArgs> ControllerDisabled;

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            foreach (var input in inputs)
            {
                inputState.SetButtonValue(input.button, input.value);
            }
        }
    }

}