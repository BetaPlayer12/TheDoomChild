using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DChild.Gameplay.Cinematics
{
    [SerializeField]
    public class ParticleSystemHandleClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField, HideLabel]
        private ParticleSystemHandleBehaviour info = new ParticleSystemHandleBehaviour();

        [HideInInspector]
        public double clipStart;
        [HideInInspector]
        public double clipEnd;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ParticleSystemHandleBehaviour>.Create(graph, info);
            var behaviour = playable.GetBehaviour();
            behaviour.clipStart = clipStart;
            behaviour.clipEnd = clipEnd;
            return playable;
        }

        public ClipCaps clipCaps => ClipCaps.None;
    }
}