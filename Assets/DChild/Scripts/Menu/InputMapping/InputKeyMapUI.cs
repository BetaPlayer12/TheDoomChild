using DChild.Inputs;
using UnityEngine;

namespace DChild.Menu.Inputs
{
    [AddComponentMenu("DChild/Menu/Input/Input Key Map UI")]
    public class InputKeyMapUI : MonoBehaviour
    {
        [SerializeField]
        private InputMapper m_mapper;
        private InputKeyMapButton[] m_buttons;

        private InputKeyMapButton m_toEdit;

        private KeyCode[] m_keyCodeList;

        public void EditInput(InputKeyMapButton button)
        {
            m_toEdit = button;
            m_toEdit.SetInteractability(false);
            enabled = true;
        }

        public void UpdateButtons()
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                m_buttons[i].SetKeyCode(m_mapper.proposedMap.GetKeyCode(m_buttons[i].inputKey));
            }
        }

        private void Awake()
        {
            m_buttons = GetComponentsInChildren<InputKeyMapButton>();
            m_keyCodeList = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
            m_mapper.Initialize();
            m_mapper.CopyCurrentMap();
            UpdateButtons();
            enabled = false;
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < m_keyCodeList.Length; i++)
                {
                    var keycode = m_keyCodeList[i];
                    if (Input.GetKeyDown(keycode))
                    {
                        m_toEdit.SetKeyCode(keycode);
                        m_mapper.proposedMap.SetInputKeyCode(m_toEdit.inputKey, keycode);
                        m_toEdit.SetInteractability(true);
                        m_toEdit = null;
                        enabled = false;
                    }
                }
            }
        }
    }
}
