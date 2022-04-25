using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DChild.UI
{
    public class DialogueSkipHandle : MonoBehaviour
    {
        [SerializeField]
        private DialogueSkipProgressionUI m_ui;
        [SerializeField]
        private float m_holdButtonDuration;

        private float m_holdButtonDurationTimer;

        private void OnEnable()
        {
            m_holdButtonDurationTimer = 0;
            m_ui.SetProgression(0);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_holdButtonDurationTimer += Time.unscaledDeltaTime;
                m_ui.SetProgression(m_holdButtonDurationTimer / m_holdButtonDuration);
                if (m_holdButtonDurationTimer >= m_holdButtonDuration)
                {
                    m_holdButtonDurationTimer = 0;
                    m_ui.SetProgression(1);
                    DialogueManager.StopConversation();
                    enabled = false;
                }
            }
            else
            {
                m_holdButtonDurationTimer = 0;
                m_ui.SetProgression(0);
            }
        }
    }
}