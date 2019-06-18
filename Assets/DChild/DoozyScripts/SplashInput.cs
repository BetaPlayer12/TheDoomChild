using Doozy.Engine;
using UnityEngine;

namespace DChild.Menu
{
    public class SplashInput : MonoBehaviour
    {
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                GameEventMessage.SendEvent("Splash Hide");
                enabled = false;
            }
        }
    }
}