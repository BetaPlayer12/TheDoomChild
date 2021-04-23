using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    public class ParticleSystemHandleMixer : PlayableBehaviour
    {
        public ParticleSystem particleSystem { get; set; }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

        }

        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
        }

        private void PrepareParticle(ScriptPlayable<ParticleSystemHandleBehaviour> playable)
        {
            if (particleSystem.useAutoRandomSeed)
                particleSystem.useAutoRandomSeed = false;

            var behaviour = playable.GetBehaviour();
            if (particleSystem.randomSeed != behaviour.seed)
            {
                particleSystem.randomSeed = behaviour.seed;
            }

            var main = particleSystem.main;
            main.duration = (float)playable.GetDuration();
        }
    }
}