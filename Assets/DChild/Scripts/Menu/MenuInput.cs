using Holysoft.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu
{

    public class MenuInput : IMenuInput
    {

        public bool backTrack { get; private set; }
        public bool enabled { get; set; }

        public MenuInput()
        {
            enabled = true;
        }

        public void Update()
        {
            if (enabled)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    backTrack = true;
                }
            }
        }

        public void LateUpdate()
        {
            if (enabled)
            {
                backTrack = false;
            }
        }
    }

}