using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyCharacter", menuName = "DChild/Gameplay/Army/Character")]
    public class ArmyCharacter : ScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private Sprite m_image;
        [SerializeField,DisableInInlineEditors]
        private UnitType m_unitType;
        [SerializeField]
        private int m_power;

        public string name => m_name;
        public Sprite image => m_image;
        public UnitType unitType => m_unitType;
        public int power => m_power;
    }
}