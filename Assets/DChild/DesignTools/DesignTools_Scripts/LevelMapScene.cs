using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChildEditor.DesignTool.LevelMap
{
    public class LevelMapScene : MonoBehaviour
    {
        [SerializeField, MinValue(1)]
        private int m_sceneNumber = 1;
        [SerializeField, ChildGameObjectsOnly, ValueDropdown("GetChildren", IsUniqueList = true)]
        private LevelMapElementGroup[] m_elements;

        private bool isPopulated => m_elements.Length > 0;

        private IEnumerable GetChildren()
        {
            return GetComponentsInChildren<LevelMapElement>(true);
        }

        [Button]
        private void PopulateList()
        {
            m_elements = GetComponentsInChildren<LevelMapElementGroup>(true);
        }

        [Button, ShowIf("isPopulated")]
        private void RenameElements()
        {
            var prefix = $"{m_sceneNumber}_";
            for (int i = 0; i < m_elements.Length; i++)
            {
                m_elements[i].RenameElements(prefix);
            }
        }


        [Button, HorizontalGroup("IndexOptions"), ShowIf("isPopulated")]
        private void LockElementIndexes()
        {
            for (int i = 0; i < m_elements.Length; i++)
            {
                m_elements[i].LockElementIndexes();
            }

        }

        [Button, HorizontalGroup("IndexOptions"), ShowIf("isPopulated")]
        private void UnlockElementIndexes()
        {
            for (int i = 0; i < m_elements.Length; i++)
            {
                m_elements[i].UnlockElementIndexes();
            }
        }
    }
}