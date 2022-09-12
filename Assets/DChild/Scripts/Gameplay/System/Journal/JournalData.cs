using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems.Journal
{
    [CreateAssetMenu(fileName = "JournalData", menuName = "DChild/Database/Journal Data")]
    public class JournalData : ScriptableObject
    {
        [SerializeField]
        private int m_id;
        [SerializeField]
        private Sprite m_notification;

        public int ID => m_id;
        public Sprite notification => m_notification;
    }
}
