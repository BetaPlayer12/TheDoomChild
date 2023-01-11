using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyCharacter", menuName = "DChild/Gameplay/Army/Character")]
    public class ArmyCharacter : ScriptableObject
    {
        [HorizontalGroup("Line")]
        [SerializeField, DisableInInlineEditors, LabelWidth(57)]
        private Sprite m_image;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert")]
        private string m_name;
        [SerializeField, HideInTables, DisableInInlineEditors, VerticalGroup("Line/Vert")]
        private UnitType m_unitType;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert")]
        private int m_power;

        public Sprite image => m_image;
        public string name => m_name;
        public UnitType unitType => m_unitType;
        public int power => m_power;
    }
}