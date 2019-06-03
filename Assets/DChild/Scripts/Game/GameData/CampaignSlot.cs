using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Holysoft.Collections;
using DChild.Gameplay.Environment;

namespace DChild.Serialization
{
    [System.Serializable]
    public class CampaignSlot
    {
        [SerializeField, ReadOnly]
        private int m_id;
        [SerializeField, OnValueChanged("OnNewGameChange")]
        private bool m_newGame;
        [SerializeField, HideIf("m_newGame")]
        private Location m_location;
        [SerializeField, HideIf("m_newGame"), MinValue(0)]
        private int m_completion;
        [SerializeField, HideIf("m_newGame")]
        private TimeKeeper m_duration;
        [SerializeField]
        private PlayerCharacterData m_characterData;

        public CampaignSlot(int m_id)
        {
            this.m_id = m_id;
            m_newGame = true;
            m_location = Location.None;
            m_completion = 0;
            m_duration = new TimeKeeper();
        }

        public int id => m_id;
        public bool newGame => m_newGame;
        public Location location => m_location;
        public int completion => m_completion;
        public TimeKeeper duration => m_duration;

        public void Reset()
        {
            m_newGame = true;
            m_location = Location.None;
            m_completion = 0;
            m_duration = new TimeKeeper();
        }

#if UNITY_EDITOR
        public void SetID(int ID)
        {
            m_id = ID;
        }

        private void OnNewGameChange()
        {
            if (m_newGame)
            {
                Reset();
            }
        }
#endif
    }

}