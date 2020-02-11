using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Inputs
{
    [System.Serializable]
    public class InputMapper
    {
        [SerializeField]
        private InputMapData m_default;
        [SerializeField]
        private InputMapData m_toEdit;

        [ShowInInspector, HideInEditorMode]
        private InputMap m_proposedMap;

        public InputMap proposedMap => m_proposedMap;

        [Button]
        public void CopyCurrentMap()
        {
            m_proposedMap.Copy(m_toEdit.inputMap.dataList);
        }

        [Button]
        public void ApplyEdit()
        {
            m_toEdit.inputMap.Copy(m_proposedMap.dataList);
        }

        [Button]
        public void RestoreDefault()
        {
            m_toEdit.inputMap.Copy(m_default.inputMap.dataList);
            m_proposedMap.Copy(m_toEdit.inputMap.dataList);
        }

        public void Initialize()
        {
            m_proposedMap = new InputMap();
        }
    }
}
