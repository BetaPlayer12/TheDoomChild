﻿using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (m_character.facing != DChild.Gameplay.Characters.HorizontalDirection.Right)
                {
                    m_character.SetFacing(DChild.Gameplay.Characters.HorizontalDirection.Right); 
                }
                isFacingRight = true;
            }
            else if (left)
            {
                inputState.direction = Directions.Left;
                if (m_character.facing != DChild.Gameplay.Characters.HorizontalDirection.Left)
                {
                    m_character.SetFacing(DChild.Gameplay.Characters.HorizontalDirection.Left);
                }
                isFacingRight = false;
            }

            
            transform.localScale = new Vector3((float)inputState.direction, 1, 1);
        }
    }

}