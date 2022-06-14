using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    [System.Serializable]
    public class PlayerSoulSkillData
    {
        [SerializeField]
        private int m_maxSoulCapacity;
        [SerializeField,ValueDropdown("GetAllSoulSkills",IsUniqueList =true)]
        private int[] m_acquiredSoulSkills;
        [SerializeField, ValueDropdown("m_acquiredSoulSkills")]
        private int[] m_activatedSoulSkills;

        public PlayerSoulSkillData()
        {
            m_acquiredSoulSkills = new int[0];
            m_activatedSoulSkills = new int[0];
            m_maxSoulCapacity = 1;
        }

        public PlayerSoulSkillData(int maxSoulCapacity, int[] acquiredSoulSkills, int[] activatedSoulSkills)
        {
            m_acquiredSoulSkills = acquiredSoulSkills;
            m_activatedSoulSkills = activatedSoulSkills;
            m_maxSoulCapacity = maxSoulCapacity;
        }

        public int maxSoulCapacity => m_maxSoulCapacity;
        public int[] acquiredSoulSkills => m_acquiredSoulSkills;
        public int[] activatedSoulSkills => m_activatedSoulSkills;

#if UNITY_EDITOR
        private IEnumerable GetAllSoulSkills()
        {
            var connection = DChildDatabase.GetSoulSkillConnection();
            connection.Initialize();
            var infoList = connection.GetAllSkills();
            connection.Close();

            var list = new ValueDropdownList<int>();
            for (int i = 0; i < infoList.Length; i++)
            {
                list.Add(infoList[i].name, infoList[i].id);
            }
            return list;
        }
#endif
    }
}
