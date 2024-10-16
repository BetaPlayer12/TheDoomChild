using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyGroupTemplateList
    {
        [SerializeField, AssetSelector]
        private ArmyGroupTemplateData[] m_list;

        public int count => m_list.Length;
        public ArmyGroupTemplateData GetData(int index) => m_list[index];
    }
}