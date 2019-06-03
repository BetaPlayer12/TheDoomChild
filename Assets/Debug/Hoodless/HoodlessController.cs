using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class HoodlessController : MonoBehaviour
    {
        private HoodlessCharacter m_hoodless;

        private void Start()
        {
            m_hoodless = GetComponent<HoodlessCharacter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_hoodless.ToggleHoodless();
            }
        }
    }
}
