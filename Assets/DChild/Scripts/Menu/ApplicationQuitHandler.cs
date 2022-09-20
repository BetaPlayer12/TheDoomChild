using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class ApplicationQuitHandler : MonoBehaviour
    {
        public void Execute()
        {
            GameSystem.RequestConfirmation(OnAffirmation, "Would you like to Quit the Game?");
        }

        private void OnAffirmation(object sender, EventActionArgs eventArgs)
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }

}