using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    public class DynamicInterpolationMixerBehaviour : PlayableBehaviour
    {
        private int m_startingIndex = 0;

        private int currentIndex = -1;
        private DynamicInterpolationBehaviour currentBehaviour = null;
        private Vector2 m_startingPosition;
        private Vector2 m_destination;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            var clipCount = playable.GetInputCount();
            for (int i = m_startingIndex; i < clipCount; i++)
            {
                ScriptPlayable<DynamicInterpolationBehaviour> inputPlayable = (ScriptPlayable<DynamicInterpolationBehaviour>)playable.GetInput(i);
                if (playable.GetInputWeight(i) >= 1)
                {
                    var transform = playerData as Transform;
                    if (i != currentIndex)
                    {
                        currentIndex = i;
                        currentBehaviour = inputPlayable.GetBehaviour();
                        m_startingPosition = transform.position;

                        m_destination = m_startingPosition;
                        if (currentBehaviour.moveX)
                        {
                            m_destination.x = currentBehaviour.destination.x;
                        }
                        if (currentBehaviour.moveY)
                        {
                            m_destination.y = currentBehaviour.destination.y;
                        }
                    }
                    //Do Something currentBehaviour;
                    Interpolate(transform, currentBehaviour, playable.GetTime());
                    m_startingIndex = i;
                    break;
                }
            }
        }

        private void Interpolate(Transform transform, DynamicInterpolationBehaviour interpolationBehaviour, double currentTime)
        {
            float inClipTime = (float)(currentTime - interpolationBehaviour.clipStart);
            var evaluatedLerp = interpolationBehaviour.interpolation.Evaluate(inClipTime);
            var destination = m_startingPosition;
            transform.position = Vector2.Lerp(m_startingPosition, m_destination, evaluatedLerp);
        }
    }
}

