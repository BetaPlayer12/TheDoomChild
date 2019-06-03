using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable]
    public class PlayerProgressData
    {
        [Title("Soul Skills")]
        [SerializeField, ValueDropdown("GetAllSoulSkills", IsUniqueList = true, NumberOfItemsBeforeEnablingSearch = 5, ExpandAllMenuItems = true), Indent]
        public List<int> m_unlockedSoulSkills;
        [Title("Inventory")]
        [SerializeField, MinValue(0)]
        private int m_soulEssenceCount;

#if UNITY_EDITOR
        private IEnumerable GetAllSoulSkills() => DChildUtility.GetSoulSkills();
#endif
    }
}