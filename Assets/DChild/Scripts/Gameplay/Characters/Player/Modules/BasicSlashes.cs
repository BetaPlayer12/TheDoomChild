using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class BasicSlashes : AttackBehaviour
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

        private List<Type> m_executedTypes;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_executedTypes = new List<Type>();
        }

        public override void Cancel()
        {
            base.Cancel();
            for (int i = 0; i < m_executedTypes.Count; i++)
            {
                var type = m_executedTypes[i];
                EnableCollision(type, false);
                PlayFXFor(type, false);
                ClearFXFor(type);
            }
            m_executedTypes.Clear();
        }

        public void EnableCollision(Type type, bool value)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.ShowCollider(value);
                    break;
                case Type.Crouch:
                    m_crouch.ShowCollider(value);
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.ShowCollider(value);
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.ShowCollider(value);
                    break;
            }

            if (value)
            {
                Record(type);
            }
            else
            {
                m_executedTypes.Remove(type);
            }
        }

        public void Execute(Type type)
        {
            m_state.canAttack = false;
            m_state.isAttacking = true;
            m_state.waitForBehaviour = true;
            m_animator.SetBool(m_animationParameter, true);
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_timer = m_groundOverhead.nextAttackDelay;
                    break;
                case Type.Crouch:
                    m_timer = m_crouch.nextAttackDelay;
                    break;
                case Type.MidAir_Forward:
                    m_timer = m_midAirForward.nextAttackDelay;
                    break;
                case Type.MidAir_Overhead:
                    m_timer = m_midAirOverhead.nextAttackDelay;
                    break;
            }
            Record(type);
        }

        public void PlayFXFor(Type type, bool play)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.PlayFX(play);
                    break;
                case Type.Crouch:
                    m_crouch.PlayFX(play);
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.PlayFX(play);
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.PlayFX(play);
                    break;
            }
        }

        public void ClearFXFor(Type type)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.ClearFX();
                    break;
                case Type.Crouch:
                    m_crouch.ClearFX();
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.ClearFX();
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.ClearFX();
                    break;
            }
        }

        public void HandleNextAttackDelay()
        {
            if (m_timer >= 0)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = -1;
                    m_state.canAttack = true;
                }
            }
        }

        private void Record(Type type)
        {
            if (m_executedTypes.Contains(type) == false)
            {
                m_executedTypes.Add(type);
            }
        }
    }
}
