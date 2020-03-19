                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemController : MonoBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_handle;

        private void Update()
        {
            if (GameplaySystem.isGamePaused == false)
            {
                if (Input.GetButtonDown("QuickItemUse"))
                {
                    m_handle.UseCurrentItem();
                }
                if (Input.GetButtonDown("QuickItemPrev"))
                {
                    m_handle.Previous();
                }
                else if (Input.GetButtonDown("QuickItemNext"))
                {
                    m_handle.Next();
                }
            }
        }
    }
}
