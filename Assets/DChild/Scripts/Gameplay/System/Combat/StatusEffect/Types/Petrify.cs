using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class Petrify : ImmobolizingStatusEffect
    {
        private SkeletonAnimation m_animation;
        public override StatusEffectType type => StatusEffectType.Petrify;

        public override void SetReciever(IStatusReciever reciever)
        {
            base.SetReciever(reciever);
            m_animation = reciever.GetComponentInChildren<SkeletonAnimation>();
        }

        public override void StartEffect()
        {
            base.StartEffect();
            m_animation.skeleton.SetColor(Color.gray);
            m_animation.LateUpdate();
            m_animation.enabled = false;
        }

        public override void StopEffect()
        {
            base.StopEffect();
            m_animation.skeleton.SetColor(Color.white);
            m_animation.enabled = true;
        }
    }
}