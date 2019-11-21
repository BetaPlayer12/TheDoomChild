using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Trohpies
{
    [System.Serializable]
    public struct TrophyModuleID
    {
        [SerializeField, ReadOnly]
        private int m_trophyID;
        [SerializeField, ReadOnly]
        private int m_moduleID;

        public int trophyID => m_trophyID;
        public int moduleID => m_moduleID;
    }
}