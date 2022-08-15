using DChild.Gameplay.Characters.Players.Modules;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowCloneAttackFX : AttackBehaviour
    {
        public enum Type
        {
            Ground_Overhead,
            Crouch,
            MidAir_Forward,
            MidAir_Overhead
        }

        [SerializeField]
        private SkeletonAnimation m_attackFX;
        [SerializeField]
        private Info m_groundOverhead;
        [SerializeField]
        private Info m_crouch;
        [SerializeField]
        private Info m_midAirForward;
        [SerializeField]
        private Info m_midAirOverhead;
        [SerializeField]
        private int m_slashStateAmount;
        [SerializeField]
        private List<Info> m_slashComboInfo;

        private List<Type> m_executedTypes;
        private Animator m_fxAnimator;
        private int m_currentSlashState;
        private bool m_attacking;

        public void PlayFXFor(Type type, bool play)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.PlayFX(play);
                    m_attackFX.transform.position = m_groundOverhead.fxPosition.position;
                    m_fxAnimator.SetTrigger("GroundOverhead");
                    break;
                case Type.Crouch:
                    m_crouch.PlayFX(play);
                    m_attackFX.transform.position = m_crouch.fxPosition.position;
                    m_fxAnimator.SetTrigger("Crouch");
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.PlayFX(play);
                    m_attackFX.transform.position = m_midAirForward.fxPosition.position;
                    m_fxAnimator.Play("JumpSlash");
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.PlayFX(play);
                    m_attackFX.transform.position = m_midAirOverhead.fxPosition.position;
                    m_fxAnimator.SetTrigger("JumpOverhead");
                    break;
            }
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

        public void FinishAttackAnim()
        {
            ClearExecutedCollision();
        }

        public void FinishSlashComboAttackAnim()
        {
            for (int i = 0; i < m_slashComboInfo.Count; i++)
            {
                m_slashComboInfo[i].ShowCollider(false);
            }

            if (m_currentSlashState >= m_slashStateAmount)
            {
                m_currentSlashState = 0;
            }

            m_fxAnimator.Play("Buffer");
        }

        private void Record(Type type)
        {
            if (m_executedTypes.Contains(type) == false)
            {
                m_executedTypes.Add(type);
            }
        }

        public void ClearExecutedCollision()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                EnableCollision(type, false);
            }

            m_executedTypes.Clear();
        }

        public void EnableSlashComboCollision(bool value, int slashState)
        {
            m_attacking = true;
            m_slashComboInfo[slashState].ShowCollider(value);
            m_attackFX.transform.position = m_slashComboInfo[slashState].fxPosition.position;
        }

        public void ComboEnd()
        {
            if (m_attacking == false)
            {
                m_currentSlashState = 0;
            }
        }

        void Start()
        {
            m_currentSlashState = 0;
            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_executedTypes = new List<Type>();
            m_attacking = false;
        }
    }
}
