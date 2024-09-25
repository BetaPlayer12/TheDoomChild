using System.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackDeathBloodLightingSequenceHandle : BlackDeathBloodLightingBehaviourHandle
    {
        [System.Serializable]
        private class Sequence
        {
            [SerializeField]
            private float m_delay;
            [SerializeField]
            private bool m_afterPreviousSequenceDone;
            [SerializeField]
            private BlackDeathBloodLightning[] m_lightnings;

            public bool isAfterPreviousSequenceDone => m_afterPreviousSequenceDone;
            public event EventAction<EventActionArgs> IsDone;

            public IEnumerator Execute()
            {
                m_lightnings[0].IsDone += OnIsDone;

                if (m_delay > 0)
                {
                    yield return new WaitForSeconds(m_delay);
                }
                for (int i = 0; i < m_lightnings.Length; i++)
                {
                    m_lightnings[i].Execute();
                }
            }

            private void OnIsDone(object sender, EventActionArgs eventArgs)
            {
                ((BlackDeathBloodLightning)sender).IsDone -= OnIsDone;
                IsDone?.Invoke(this, EventActionArgs.Empty);
            }
        }

        [SerializeField]
        private Sequence[] m_sequences;

        private bool m_isExecutinSequence;

        [Button, HideInEditorMode]
        public override void Execute()
        {
            if (m_isExecutinSequence)
                return;

            StopAllCoroutines();
            StartCoroutine(PlaySequencesRoutine());
        }

        private IEnumerator PlaySequencesRoutine()
        {
            m_isExecutinSequence = true;

            yield return m_sequences[0].Execute();
            for (int i = 1; i < m_sequences.Length; i++)
            {
                var sequence = m_sequences[i];
                if (sequence.isAfterPreviousSequenceDone)
                {
                    yield return WaitForSequenceToBeDone(m_sequences[i - 1]);
                }
                yield return sequence.Execute();
            }

            m_isExecutinSequence = false;
        }

        private IEnumerator WaitForSequenceToBeDone(Sequence sequence)
        {
            bool isDone = false;
            sequence.IsDone += IsDone;
            while (isDone == false)
            {
                yield return null;
            }

            void IsDone(object sender, EventActionArgs eventArgs)
            {
                sequence.IsDone -= IsDone;
                isDone = true;
            }
        }
    }

}