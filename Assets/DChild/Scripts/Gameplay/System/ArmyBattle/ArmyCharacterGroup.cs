using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{
    public abstract class ArmyCharacterGroup
    {
        protected List<ArmyCharacterData> m_armyGroupList;
        [ShowInInspector]
        private bool m_isAvailable;

        public ArmyCharacterGroup()
        {
            m_armyGroupList = new List<ArmyCharacterData>();
            m_isAvailable = true;
        }
        public ArmyCharacterGroup(ArmyCharacterGroup reference)
        {
            m_armyGroupList = new List<ArmyCharacterData>();
            for (int i = 0; i < reference.armyGroupCount; i++)
            {
                m_armyGroupList.Add(reference.GetAvailableMember(i));
            }
            m_isAvailable = true;
        }


        [ShowInInspector, PropertyOrder(0)]
        public abstract string name { get; }
        public int armyGroupCount => m_armyGroupList.Count;
        public bool isAvailable => m_isAvailable;

        public ArmyCharacterData GetAvailableMember(int index) => m_armyGroupList[index];

        public void SetAvailability(bool isAvailable)
        {
            m_isAvailable = isAvailable;
        }
    }
}