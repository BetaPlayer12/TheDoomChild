using Holysoft.Event;
using Holysoft.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.MainMenu
{
    public class MainMenuScreen : UIStylishCanvas
    {
        public void OnExitAffirmed(object sender, EventActionArgs eventArgs)
        {
            Application.Quit();
        }

        public void RequestExit()
        {
            MenuSystem.RequestConfirmation(OnExitAffirmed, "<font=\"BACKCOUNTRY-Regular SDF\">Do you really want to exit the game?");
        }

    }
}