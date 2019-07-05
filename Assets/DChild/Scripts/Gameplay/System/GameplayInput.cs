using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class GameplayInput : MonoBehaviour
    {
        [SerializeField]
        private KeyCode m_pause;


        private void Update()
        {
            if (Input.GetKeyDown(m_pause))
            {
                GameEventMessage.SendEvent("Pause Game");
            }
        }
    }
}