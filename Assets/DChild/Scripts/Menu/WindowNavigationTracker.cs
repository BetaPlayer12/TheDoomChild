using Holysoft.Menu;
using Holysoft.UI;
using UnityEngine;

namespace DChild.Menu
{
    [RequireComponent(typeof(IMenuNavigation))]
    public class WindowNavigationTracker : UIBehaviour
    {
        private IMenuNavigation m_navigation;

        private void OnCanvasOpen(object sender, UICanvas eventArgs)
        {
            MenuSystem.backTracker.Stack(eventArgs);
        }

        private void Awake()
        {
            m_navigation = GetComponent<IMenuNavigation>();
        }

        private void Start()
        {
            if (MenuSystem.backTracker.HasStacked(m_navigation.mainCanvas) == false)
            {
                MenuSystem.backTracker.Stack(m_navigation.mainCanvas);
            }
        }

        private void OnEnable()
        {
            m_navigation.CanvasOpen += OnCanvasOpen;
        }

        private void OnDisable()
        {
            m_navigation.CanvasOpen -= OnCanvasOpen;
        }
    }

}