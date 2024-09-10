using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [CreateAssetMenu(fileName = "ArmyCharacter", menuName = "DChild/Gameplay/Army/Character")]
    public class ArmyCharacterData : ScriptableObject
    {
        [SerializeField, HideInInlineEditors]
        private int m_ID;
        [HorizontalGroup("Line")]
        [SerializeField, DisableInInlineEditors, LabelWidth(57)]
        private Sprite m_icon;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert")]
        private string m_name;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert"), MinValue(1), HideInInlineEditors]
        private int m_troopCount = 1;
        [SerializeField, DisableInInlineEditors, VerticalGroup("Line/Vert"), MinValue(1)]
        private int m_attackPower = 1;


        public ArmyCharacterData()
        {
            m_ID = -1;
            m_icon = null;
            m_troopCount = -1;
            m_attackPower = -1;
            m_name = null;
        }

        public ArmyCharacterData(int iD, Sprite image, string name, int troopCount, int power)
        {
            m_ID = iD;
            m_icon = image;
            m_name = name;
            m_troopCount = troopCount;
            m_attackPower = power;
        }

        public int ID => m_ID;
        public Sprite image => m_icon;
        public string name => m_name;
        public int troopCount => m_troopCount;
        public int power => m_attackPower;
    }
}