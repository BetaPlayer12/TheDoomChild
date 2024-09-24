using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Systems.Journal
{
    public class JournalProgress : MonoBehaviour
    {
        public struct SaveData : ISaveData
        {
            [SerializeField]
            private int[] m_recordedJournalIDs;

            public SaveData(IReadOnlyCollection<int> m_recordedJournalIDs)
            {
                this.m_recordedJournalIDs = m_recordedJournalIDs.ToArray();
            }

            public int[] recordedJournalIDs => m_recordedJournalIDs;

            ISaveData ISaveData.ProduceCopy() => new SaveData(m_recordedJournalIDs);
        }

        private List<int> m_recordedJournalIDs;

        public void UpdateJournal(JournalData journaldata)
        {
            if (IsNewProgress(journaldata))
            {
                m_recordedJournalIDs.Add(journaldata.ID);
                GameplaySystem.gamplayUIHandle.notificationManager.ShowJournalUpdateNotification(journaldata);
                //m_notification.UpdateNotification(journaldata);
            }
        }

        public bool IsNewProgress(JournalData journaldata) => m_recordedJournalIDs.Contains(journaldata.ID) == false;

        public ISaveData Save()
        {
            return new SaveData(m_recordedJournalIDs);
        }

        public void Load(ISaveData data)
        {
            var saveData = (SaveData)data;
            if (m_recordedJournalIDs == null)
            {
                m_recordedJournalIDs = new List<int>(saveData.recordedJournalIDs);
            }
            else
            {
                m_recordedJournalIDs.Clear();
                m_recordedJournalIDs.AddRange(saveData.recordedJournalIDs);
            }
        }

        private void Awake()
        {
            if (m_recordedJournalIDs == null)
            {
                m_recordedJournalIDs = new List<int>();
            }
        }
    }
}
