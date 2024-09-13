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


        public ArmyCharacter()
        {
            m_ID = -1;
            m_image = null;
            m_troopCount = -1;
            m_power = -1;
            m_name = null;
        }

        public ArmyCharacter(int iD, Sprite image, string name, int troopCount, int power)
        {
            m_ID = iD;
            m_image = image;
            m_name = name;
            m_troopCount = troopCount;
            m_power = power;
        }

        public int ID => m_ID;
        public Sprite image => m_image;
        public string name => m_name;
        public int troopCount => m_troopCount;
        public int power => m_power;
    }
}