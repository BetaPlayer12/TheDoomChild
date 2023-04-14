using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DChildEditor.DesignTool.LevelMap
{
    public class LevelMapElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_label;

        [SerializeField, FoldoutGroup("Configuration")]
        private bool m_lockIndex;
        [SerializeField, ShowIf("lockIndex"), FoldoutGroup("Configuration")]
        private int m_index;
        [SerializeField, ReadOnly]
        private string m_elementName;

        public bool lockIndex => m_lockIndex;
        public int index => m_index;
        public string elementName => m_elementName;

        public void SetDisplay(string prefix, int index)
        {
            var name = prefix;

            if (m_lockIndex == false)
            {
                m_index = index;
            }
            name += m_index;

            m_elementName = name;
            gameObject.name = m_elementName;
            m_label.text = m_elementName;
            m_label.gameObject.name = m_elementName + "(Label)";
        }

        public void SetLockIndex(bool lockIndex)
        {
            m_lockIndex = lockIndex;
        }
    }
}