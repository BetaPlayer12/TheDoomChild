using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class StealthController : MonoBehaviour
    {
        private StealthCharacter m_stealth;

        private void Start()
        {
            m_stealth = GetComponent<StealthCharacter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_stealth.ToggleStealth();
            }
        }
    }
}