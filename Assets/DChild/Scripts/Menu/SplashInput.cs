using Doozy.Engine;
using UnityEngine;

namespace DChild.Menu
{
    public class SplashInput : MonoBehaviour
    {
        private void Start()
        {
            Debug.LogError("False Positive");
        }

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