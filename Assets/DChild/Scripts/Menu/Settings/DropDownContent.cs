using Holysoft.Event;
using Holysoft.UI;
using TMPro;
using UnityEngine;

namespace DChild.Menu.UI
{
    public class DropDownContent : MonoBehaviour
    {
        [SerializeField]
        private UICanvas[] m_parentCanvases;
        [SerializeField]
        private TMP_Dropdown m_dropDown;
        private Canvas m_canvas;

        private void OnCanvasHide(object sender, EventActionArgs eventArgs)
        {
            m_dropDown.Hide();
        }

        private void Awake()
        {
            for (int i = 0; i < m_parentCanvases.Length; i++)
            {
                m_parentCanvases[i].CanvasHide += OnCanvasHide;
            }
        }

        private void Start()
        {
            m_canvas = GetComponent<Canvas>();
            m_canvas.overrideSorting = false;
        }
    }
}