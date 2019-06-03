#if UNITY_EDITOR
using DChild.Menu;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Menu.MainMenu
{
    public class MenuDebugger : MonoBehaviour
    {
        [System.Serializable]
        private class CanvasInfo
        {
            public EventAction<EventActionArgs> CanvasOpen;

            [BoxGroup("Info")]
            [SerializeField]
            [ReadOnly]
            private bool m_isOpen;
            [BoxGroup("Info")]
            [SerializeField]
            private UICanvas m_mainCanvas;

            public bool isOpen => m_isOpen;

            [FoldoutGroup("Info/Others")]
            [PropertySpace]
            [PropertyOrder(1)]
            [SerializeField]
            private UICanvas[] m_toOpen;

            [FoldoutGroup("Info/Others")]
            [PropertySpace]
            [PropertyOrder(1)]
            [SerializeField]
            private UICanvas[] m_toTrack;


            [BoxGroup("Info")]
            [Button("Open")]
            public void Open()
            {
                CanvasOpen?.Invoke(this, EventActionArgs.Empty);
                m_isOpen = true;
                UIUtility.OpenCanvas(m_mainCanvas);
                for (int i = 0; i < (m_toOpen?.Length ?? 0); i++)
                {
                    UIUtility.OpenCanvas(m_toOpen[i]);
                }
            }

            public void Close()
            {
                m_isOpen = false;
                m_mainCanvas.Hide();
                UIUtility.CloseCanvas(m_mainCanvas);
                for (int i = 0; i < (m_toOpen?.Length ?? 0); i++)
                {
                    UIUtility.CloseCanvas(m_toOpen[i]);
                }
            }

            public void TrackCanvases()
            {
                MenuSystem.backTracker.ClearStack();
                for (int i = 0; i < m_toTrack.Length; i++)
                {
                    MenuSystem.backTracker.Stack(m_toTrack[i]);
                }
            }
        }

        [SerializeField]
        private CanvasInfo[] m_canvases;

        private void OnCanvasOpen(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < m_canvases.Length; i++)
            {
                m_canvases[i].Close();
            }
        }

        private void Start()
        {
            for (int i = 0; i < m_canvases.Length; i++)
            {
                if (m_canvases[i].isOpen)
                {
                    m_canvases[i].TrackCanvases();
                    break;
                }
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < m_canvases.Length; i++)
            {
                m_canvases[i].CanvasOpen = OnCanvasOpen;
            }
        }
    }
}
#endif