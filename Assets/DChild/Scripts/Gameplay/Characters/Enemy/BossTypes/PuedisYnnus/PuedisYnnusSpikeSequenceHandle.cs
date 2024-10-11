using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{

    public class PuedisYnnusSpikeSequenceHandle : MonoBehaviour
    {
        [System.Serializable]
        private class SequenceInfo
        {
            [SerializeField]
            private int m_beatDelay;
            [SerializeField]
            private PuedisYnnusSpikeBeat[] m_spikesToGrow;

            public int beatDelay => m_beatDelay;
            public PuedisYnnusSpikeBeat[] spikesToGrow => m_spikesToGrow;
        }

        [SerializeField]
        private HeartBeatHandle m_source;
        [SerializeField]
        private int m_startingBeatIndex = 0;
        [SerializeField, TableList]
        private SequenceInfo[] m_sequence;


        private bool m_isSequenceOngoing;
        private bool m_isSequenceWaiting;
        private int m_sequenceIndex;
        private SequenceInfo m_currentSequence;
        private int m_beatCount;

        public event EventAction<EventActionArgs> SequenceEnd;

        [Button]
        public void ExecuteSequence()
        {
            if (m_isSequenceOngoing)
                return;

            m_source.OnBeat += OnHeartBeat;
            m_sequenceIndex = 0;
            m_currentSequence = m_sequence[m_sequenceIndex];
            m_beatCount = 0;
            m_isSequenceOngoing = true;
            m_isSequenceWaiting = true;
        }

        private void OnHeartBeat(int beatIndex)
        {
            if (m_isSequenceWaiting)
            {
                if (m_startingBeatIndex == 0 || m_startingBeatIndex == beatIndex)
                {
                    m_isSequenceWaiting = false;
                }
            }

            if (m_isSequenceWaiting)
                return;

            if (m_currentSequence.beatDelay == m_beatCount)
            {
                for (int i = 0; i < m_currentSequence.spikesToGrow.Length; i++)
                {
                    m_currentSequence.spikesToGrow[i].ProgressGrowth();
                }

                m_sequenceIndex++;
                m_beatCount = 0;

                if (m_sequenceIndex == m_sequence.Length)
                {
                    m_isSequenceOngoing = false;
                    m_source.OnBeat -= OnHeartBeat;
                    SequenceEnd?.Invoke(this, EventActionArgs.Empty);
                }
                else
                {
                    m_currentSequence = m_sequence[m_sequenceIndex];
                }
            }
            else
            {
                m_beatCount++;
            }
        }
    }
}