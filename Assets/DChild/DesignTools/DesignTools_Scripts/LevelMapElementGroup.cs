using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DChildEditor.DesignTool.LevelMap
{
    public class LevelMapElementGroup : MonoBehaviour
    {
        [SerializeField, ValueDropdown("GetReferenceName"), PropertyOrder(-2)]
        private string m_elementName;
        [SerializeField, ChildGameObjectsOnly, ValueDropdown("GetChildren", IsUniqueList = true)]
        private LevelMapElement[] m_elements;

        private bool isPopulated => m_elements.Length > 0;

        private IEnumerable GetReferenceName() => LevelMapManager.instance.GetReferenceName();

        private IEnumerable GetChildren()
        {
            return GetComponentsInChildren<LevelMapElement>(true);
        }

        [Button, PropertyOrder(-1)]
        private void RenameGameobject()
        {
            gameObject.name = LevelMapManager.instance.GetName(m_elementName).Replace(" ", "");
        }

        [Button, FoldoutGroup("Options")]
        private void PopulateList()
        {
            m_elements = GetComponentsInChildren<LevelMapElement>(true);
        }

        [Button, ShowIf("isPopulated"), FoldoutGroup("Options")]
        public void RenameElements(string prefix)
        {
            var prefixName = $"{prefix}{LevelMapManager.instance.GetID(m_elementName)}_";

            var lockedIndexes = m_elements.Where(x => x.lockIndex).Select(x => x.index).ToList();
            int currentIndex = 1;
            for (int i = 0; i < m_elements.Length; i++)
            {
                var element = m_elements[i];
                if (element.lockIndex)
                {
                    element.SetDisplay(prefixName, currentIndex);
                }
                else
                {
                    while (CanApplyIndex(currentIndex) == false)
                    {
                        currentIndex++;
                    }
                    element.SetDisplay(prefixName, currentIndex);
                    currentIndex++;
                }
            }

            bool CanApplyIndex(int index)
            {
                return lockedIndexes.Contains(index) == false;
            }
        }


        [Button, HorizontalGroup("Options/IndexOptions"), ShowIf("isPopulated")]
        public void LockElementIndexes()
        {
            for (int i = 0; i < m_elements.Length; i++)
            {
                m_elements[i].SetLockIndex(true);
            }

        }

        [Button, HorizontalGroup("Options/IndexOptions"), ShowIf("isPopulated")]
        public void UnlockElementIndexes()
        {
            for (int i = 0; i < m_elements.Length; i++)
            {
                m_elements[i].SetLockIndex(false);
            }
        }
    }
}