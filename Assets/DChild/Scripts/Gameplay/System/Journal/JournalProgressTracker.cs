using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.System.Journal
{
    public class JournalProgressTracker : MonoBehaviour
    {
        public struct SaveData : ISaveData
        {
            [SerializeField]
            public int[] m_recordedJournalIDs;
            public SaveData(int[] m_recordedJournalIDs)
            {
                this.m_recordedJournalIDs = m_recordedJournalIDs;
            }
            ISaveData ISaveData.ProduceCopy() => new SaveData(m_recordedJournalIDs);
        }
        [SerializeField]
        public int[] m_recordedJournalIDs;
        [SerializeField]
        public JournalNotification m_notification;
        public bool m_isrecorded;

        public void UpdateJournal(JournalData journaldata)
        {

        }

        public void IsNewProgress(JournalData journaldata)
        {
            if (m_isrecorded == true)
            {

            }
            else
            {
                m_isrecorded = true;
            }
        }

        public ISaveData Save()
        {
            return new SaveData(m_recordedJournalIDs);
        }

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            m_recordedJournalIDs = saveData.m_recordedJournalIDs;
        }
    }
}
