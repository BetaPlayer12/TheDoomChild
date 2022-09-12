using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.NPC
{

    [CreateAssetMenu(fileName = "NPCProfile", menuName = "DChild/Database/NPC Profile")]
    public class NPCProfile : ScriptableObject
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private string m_title;
        [SerializeField,PreviewField]
        private Sprite m_baseIcon;

        public string characterName => m_name;
        public string title => m_title;
        public Sprite baseIcon => m_baseIcon;
    }
}