using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters;

namespace PlayerNew
{
    public class FaceDirection : PlayerBehaviour
    {
        [SerializeField]
        private Character m_character;

        // Start is called before the first frame update
        public bool isFacingRight;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var right = inputState.GetButtonValue(inputButtons[0]);
            var left = inputState.GetButtonValue(inputButtons[1]);

            if (right)
            {
                inputState.direction = Directions.Right;
                if (isFacingRight == false)
                {
                    isFacingRight = true;
                    m_character.SetFacing(HorizontalDirection.Right);
                }
            }
            else if (left)
            {
                inputState.direction = Directions.Left;
                if (isFacingRight == true)
                {
                    isFacingRight = false;
                    m_character.SetFacing(HorizontalDirection.Left);
                }
            }
            transform.localScale = new Vector3((float)inputState.direction, 1, 1);
        }
    }

}