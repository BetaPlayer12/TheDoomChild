using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [CreateAssetMenu(fileName = "ArmyCharacter", menuName = "DChild/Gameplay/Army/Character")]
    public class ArmyCharacter : ScriptableObject
    {
        [SerializeField, HideInInlineEditors]
        private int m_ID;
        [HorizontalGroup("Line")]
        [SerializeField, DisableInInlineEditors, LabelWidth(57)]
        private Sprite m_image;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert")]
        private string m_name;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert"), MinValue(1), HideInInlineEditors]
        private int m_troopCount = 1;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert"), MinValue(1)]
        private int m_power = 1;


        public int ID => m_ID;
        public Sprite image => m_image;
        public string name => m_name;
        public int troopCount => m_troopCount;
        public int power => m_power;
    }
}