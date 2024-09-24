using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackDeathBladeOfDarknessSequenceHandle : MonoBehaviour
    {
        [System.Serializable]
        private class Sequence
        {
            [SerializeField]
            private float m_delay;
            [SerializeField]
            private bool m_afterPreviousSequenceDone;
            [SerializeField]
            private BlackDeathBladeOfDarkness[] m_blades;

            public bool isAfterPreviousSequenceDone => m_afterPreviousSequenceDone;
            public event EventAction<EventActionArgs> IsDone;

            public IEnumerator Execute()
            {
                m_blades[0].IsDone += OnIsDone;

                if (m_delay > 0)
                {
                    yield return new WaitForSeconds(m_delay);
                }
                for (int i = 0; i < m_blades.Length; i++)
                {
                    m_blades[i].Execute();
                }
            }

            private void OnIsDone(object sender, EventActionArgs eventArgs)
            {
                ((BlackDeathBladeOfDarkness)sender).IsDone -= OnIsDone;
                IsDone?.Invoke(this, EventActionArgs.Empty);
            }
        }

        [SerializeField]
        private Sequence[] m_sequences;

        private bool m_isExecutingSequence;

        public bool isExecutingSequence => m_isExecutingSequence;

        [Button, HideInEditorMode]
        public void Execute()
        {
            if (m_isExecutingSequence)
                return;

            StopAllCoroutines();
            StartCoroutine(PlaySequencesRoutine());
        }

        private IEnumerator PlaySequencesRoutine()
        {
            m_isExecutingSequence = true;

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

            m_isExecutingSequence = false;
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