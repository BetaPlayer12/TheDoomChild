using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.MainMenu
{
    public class MainMenuButton : MonoBehaviour
    {

        private void OnMouseEnter()
        {
            Debug.Log($"Select: {gameObject.name}");
        }

        private void OnMouseExit()
        {
        }
    }
}