using UnityEngine;

namespace DChild.Inputs
{
    public interface IKeyInput
    {
        bool isPressed { get; }
        bool isHeld { get; }
    }

    public class KeyInput : IKeyInput, IInput
    {
        [SerializeField]
        private KeyCode m_key;

        public bool isPressed { get; private set; }
        public bool hasPressed { get; private set; }
        public bool isHeld { get; private set; }

        public void Update()
        {
            isPressed = Input.GetKeyDown(m_key);
            if (isPressed && hasPressed == false)
            {
                hasPressed = true;
                isHeld = false;
            }
            else if (hasPressed)
            {
                var currentValue = Input.GetKey(m_key);
                if (currentValue)
                {
                    isHeld = true;
                }
                else
                {
                    hasPressed = false;
                    isHeld = false;
                }
            }

        }

        public void Reset()
        {
            isPressed = false;
            hasPressed = false;
            isHeld = false;
        }
    }
}