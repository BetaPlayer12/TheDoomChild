using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class CriticalHealthHandle : CriticalStatHandle
    {
        [SerializeField]
        private UIContainer m_container;

        protected override void CancelCriticalEffects()
        {
            m_container.Hide();
        }

        protected override void OnStatAtCriticalValue()
        {
            m_container.Show();
        }
    }
}