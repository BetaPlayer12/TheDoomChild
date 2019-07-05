using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class GameplayInput : MonoBehaviour
    {
        [SerializeField]
        private KeyCode m_pause;
        [SerializeField]
        private KeyCode m_bestiary;


        private void Update()
        {
            if (Input.GetKeyDown(m_pause))
            {
                GameEventMessage.SendEvent("Pause Game");
            }
            else if (Input.GetKeyDown(m_bestiary))
            {
                GameEventMessage.SendEvent("Bestiary Open");
            }
        }
    }
}