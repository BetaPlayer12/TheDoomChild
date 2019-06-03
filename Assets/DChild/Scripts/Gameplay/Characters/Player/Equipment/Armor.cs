using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Equipments
{
    [System.Serializable]
    public class Armor : IArmor
    {
        [SerializeField, MinValue(0), BoxGroup("Armor")]
        private int m_defense;
        [SerializeField, MinValue(0), BoxGroup("Armor")]
        private int m_magicDefense;

        public int defense => m_defense;
        public int magicDefense => m_magicDefense;

        public event EventAction<EventActionArgs> ValueChanged;

#if UNITY_EDITOR
        public void Initialize(int defense, int magicDefense)
        {
            m_defense = defense;
            m_magicDefense = magicDefense;
        }
#endif
    }
}