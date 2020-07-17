using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    public class TimelineEventMixerBehaviour : PlayableBehaviour
    {
        private int m_startingIndex = 0;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            var clipCount = playable.GetInputCount();
            for (int i = m_startingIndex; i < clipCount; i++)
            {
                ScriptPlayable<TimelineEventBehaviour> inputPlayable = (ScriptPlayable<TimelineEventBehaviour>)playable.GetInput(i);
                if (inputPlayable.GetInputWeight(i) > 1)
                {
                    inputPlayable.GetBehaviour().eventToCall.Invoke();
                    m_startingIndex = i + 1;
                    break;
                }
            }
        }
    }
}

