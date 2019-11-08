using DChild.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Inputs
{
    [AddComponentMenu("DChild/Menu/Input/Input Key Map Button")]
    public class InputKeyMapButton : MonoBehaviour
    {
        [SerializeField]
        private InputKey m_inputKey;
        [SerializeField]
        private TextMeshProUGUI m_text;
        private Button m_button;

        public InputKey inputKey { get => m_inputKey; }

        public void SetInteractability(bool value) => m_button.interactable = value;
        public void SetKeyCode(KeyCode value) => m_text.text = value.ToString();

        public void Awake()
        {
            m_button = GetComponent<Button>();
        }
    }
}
