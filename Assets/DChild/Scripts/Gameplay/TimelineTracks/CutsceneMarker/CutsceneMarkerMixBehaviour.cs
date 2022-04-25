using System.Collections.Generic;
using UnityEngine.Playables;

namespace DChildDebug.Cutscene
{
    public class CutsceneMarkerMixBehaviour : PlayableBehaviour
    {
        private HashSet<int> m_playedBehaviourIndex = new HashSet<int>();
        private PlayableDirector m_director;
        private double m_endClipDuration;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0.001f && m_playedBehaviourIndex.Contains(i) == false)
                {
                    ScriptPlayable<CutsceneMarkerBehaviour> inputPlayable = (ScriptPlayable<CutsceneMarkerBehaviour>)playable.GetInput(i);
                    var behaviour = inputPlayable.GetBehaviour();
                    m_endClipDuration = behaviour.clipEnd;
                    m_playedBehaviourIndex.Add(i);
                }
                else if(inputWeight < 0.001f && m_playedBehaviourIndex.Contains(i))
                {
                    m_playedBehaviourIndex.Remove(i);
                }
            }
        }

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            m_director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
        }
    }
}
