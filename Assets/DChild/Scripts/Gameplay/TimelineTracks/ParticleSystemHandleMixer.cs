using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    public class ParticleSystemHandleMixer : PlayableBehaviour
    {
        public ParticleSystem particleSystem { get; set; }
        private int m_currentIndex = 0;
        private bool m_startingPlayed;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            var inputCount = playable.GetInputCount();
            if (m_startingPlayed)
            {
                for (int i = m_currentIndex; i < inputCount; i++)
                {
                    var clip = ((ScriptPlayable<ParticleSystemHandleBehaviour>)playable.GetInput(i)).GetBehaviour();
                    var weight = playable.GetInputWeight(i);
                    if (weight > 0)
                    {
                        if (m_currentIndex != i)
                        {
                            var lastClip = ((ScriptPlayable<ParticleSystemHandleBehaviour>)playable.GetInput(m_currentIndex)).GetBehaviour();
                            particleSystem.Stop(lastClip.affectChildren);
                            m_currentIndex = i;
                        }
                        Execute(clip, i);
                    }
                }
            }
            else
            {
                var clip = ((ScriptPlayable<ParticleSystemHandleBehaviour>)playable.GetInput(m_currentIndex)).GetBehaviour();
                var weight = playable.GetInputWeight(m_currentIndex);
                if (weight > 0)
                {
                    m_startingPlayed = true;
                    Execute(clip, 0);
                }
            }
        }

        private void Execute(ParticleSystemHandleBehaviour behaviour, int index)
        {
            switch (behaviour.action)
            {
                case ParticleSystemHandleBehaviour.HandleType.StopAtClip:
                    particleSystem.Stop(behaviour.affectChildren);
                    break;
                case ParticleSystemHandleBehaviour.HandleType.PlayWithinClip:
                    particleSystem.Play(behaviour.affectChildren);
                    break;
            }
            m_currentIndex = index;
        }
    }
}