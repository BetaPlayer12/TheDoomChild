using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Serialization
{
    public class BestiaryProgress
    {
        public struct ProgressEventArgs : IEventActionArgs
        {
            public ProgressEventArgs(int creatureID, bool hasEncountered) : this()
            {
                this.creatureID = creatureID;
                this.hasEncountered = hasEncountered;
            }

            public int creatureID { get; }
            public bool hasEncountered { get; }
        }

        private Dictionary<int, bool> m_encounterProgress;
        public event EventAction<ProgressEventArgs> ProgressUpdate;

        public BestiaryProgress(Dictionary<int, bool> m_encounterProgress)
        {
            this.m_encounterProgress = m_encounterProgress;
        }

        public void Encounter(int ID)
        {
            if (m_encounterProgress.ContainsKey(ID))
            {
                m_encounterProgress[ID] = true;
                ProgressUpdate?.Invoke(this, new ProgressEventArgs(ID, true));
            }
        }

        public bool HasEncountered(int ID) => m_encounterProgress[ID];
    }
}