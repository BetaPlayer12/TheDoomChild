using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyeTentacle : FlyingEye
    {
        private bool m_spikesExtended;

        protected override CombatCharacterAnimation animation => null;

        public void ExtendSpikes()
        {
            if (m_spikesExtended == false)
            {
                m_spikesExtended = true;
            }
        }

        public void RetractSpikes()
        {
            if (m_spikesExtended == true)
            {
                m_spikesExtended = false;
            }
        }

        public override void MoveTo(Vector2 destination)
        {
            m_movement.MoveTo(position, m_moveSpeed);
        }

        public override void Turn()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator FlinchRoutine()
        {
            throw new System.NotImplementedException();
        }

        protected override void ResetValues()
        {
            m_spikesExtended = false;
        }
    }

}