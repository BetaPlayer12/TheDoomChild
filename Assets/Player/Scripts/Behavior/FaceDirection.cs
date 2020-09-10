using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNew
{


    public class FaceDirection : PlayerBehaviour
    {
        [SerializeField]
        private Character m_character;
        public bool isFacingRight;

        //void Update()
        //{
        //    var right = inputState.GetButtonValue(inputButtons[0]);
        //    var left = inputState.GetButtonValue(inputButtons[1]);

        //    if (right)
        //    {
        //        inputState.direction = Directions.Right;
        //        isFacingRight = true;
        //        if (m_character.facing != DChild.Gameplay.Characters.HorizontalDirection.Right)
        //        {
        //            m_character.SetFacing(DChild.Gameplay.Characters.HorizontalDirection.Right);
        //        }
        //    }
        //    else if (left)
        //    {
        //        inputState.direction = Directions.Left;
        //        isFacingRight = false;
        //        if (m_character.facing != DChild.Gameplay.Characters.HorizontalDirection.Left)
        //        {
        //            m_character.SetFacing(DChild.Gameplay.Characters.HorizontalDirection.Left);
        //        }
        //    }

        //    transform.localScale = new Vector3((float)inputState.direction, 1, 1);
        //}

        public void UpdateFacing(float direction)
        {
            Vector3 scale = transform.localScale;

            if (direction == 1)
            {
                scale.x = 1;
            }
            else if (direction == -1)
            {
                scale.x = -1; 
            }

            transform.localScale = scale;
        }
    }
}