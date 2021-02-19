using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Narrative
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private TutorialUI m_ui;

        private TutorialInfo[] m_infos;

        private void OnSelected(object sender, EventActionArgs eventArgs)
        {
            var info = (TutorialInfo)sender;
            m_ui.SetMessage(info.instructions);
        }

        private void Start()
        {
            m_infos = GetComponentsInChildren<TutorialInfo>();
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].Selected += OnSelected;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].Selected -= OnSelected;
            }
        }
    }
}
