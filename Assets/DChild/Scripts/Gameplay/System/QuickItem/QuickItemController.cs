using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemController : MonoBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_handle;
        private string m_useButton = "QuickItemUse";
        private string m_prevButton = "QuickItemPrev";
        private string m_nextButton = "QuickItemNext";

        private bool m_hasPressed;
        private string m_pressedButton;

        private void Update()
        {
            if (m_hasPressed)
            {
                if (Input.GetButtonUp(m_pressedButton))
                {
                    m_hasPressed = false;
                }
            }
            else
            {
                if (GameplaySystem.isGamePaused == false && m_handle.hideUI == false)
                {
                    if (Input.GetButtonDown(m_useButton))
                    {
                        m_handle.UseCurrentItem();
                        m_pressedButton = m_useButton;
                    }
                    if (Input.GetButtonDown(m_prevButton))
                    {
                        m_handle.Previous();
                        m_pressedButton = m_prevButton;
                    }
                    else if (Input.GetButtonDown(m_nextButton))
                    {
                        m_handle.Next();
                        m_pressedButton = m_prevButton;
                    }
                }
            }
        }
    }
}
