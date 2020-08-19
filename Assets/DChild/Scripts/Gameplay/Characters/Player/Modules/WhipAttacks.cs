using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WhipAttacks : AttackBehaviour
    {
        public enum Type
        {
            Ground_Overhead,
            Crouch,
            MidAir_Forward,
            MidAir_Overhead
        }

        [SerializeField]
        private Info m_groundOverhead;
        [SerializeField]
        private Info m_crouch;
        [SerializeField]
        private Info m_midAirForward;
        [SerializeField]
        private Info m_midAirOverhead;

        private int m_whipAttackAnimationParameter;
        private List<Type> m_executedTypes;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_executedTypes = new List<Type>();
            m_whipAttackAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipAttack);
        }


    }
}
