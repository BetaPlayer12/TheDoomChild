using DChild.Gameplay.Characters.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PlayerImitator : MonoBehaviour
    {
        private struct AnimationParameterInfo
        {
            private float m_speedX;
            private float m_speedY;

            public AnimationParameterInfo(Animator animator, AnimationParametersData animationParametersData)
            {
                animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
                m_speedX = animator.GetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX));
                m_speedY = animator.GetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY));
            }

            public void Apply(Animator animator, AnimationParametersData animationParametersData)
            {
                animator.SetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX), m_speedX);
                animator.SetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY), m_speedY);
            }
        }

        [SerializeField]
        private AnimationParametersData m_animationParametersData;
        [SerializeField]
        private LineRenderer m_lineConnection;
        [SerializeField]
        private float m_lineYOffset;

        [SerializeField]
        private Transform m_toImitate;
        [SerializeField]
        private float m_imitateDelay;

        private Animator m_animator;
        private Animator m_animatorToImitate;

        private float m_imitationDelayTimer;
        private bool m_isDelayed;
        private List<Vector3> m_positionToImitate;
        private List<Vector3> m_scaleToImitate;
        private List<AnimationParameterInfo> m_animationToImitate;

        public void StartImitating(Player toImitate)
        {
            m_toImitate = toImitate.character.transform;
            m_animator = GetComponent<Animator>();
            m_animatorToImitate = toImitate.character.GetComponentInChildren<Animator>();
            ResetImitation();
        }

        public void ResetImitation()
        {
            transform.position = m_toImitate.position;
            m_imitationDelayTimer = m_imitateDelay;
            m_isDelayed = true;

            if (m_positionToImitate == null)
            {
                m_positionToImitate = new List<Vector3>();
                m_scaleToImitate = new List<Vector3>();
                m_animationToImitate = new List<AnimationParameterInfo>();
            }
            m_positionToImitate.Clear();
        }

        private void UpdateImitation()
        {
            transform.position = m_positionToImitate[0];
            transform.localScale = m_scaleToImitate[0];
            m_animationToImitate[0].Apply(m_animator, m_animationParametersData);

            m_positionToImitate.RemoveAt(0);
            m_scaleToImitate.RemoveAt(0);
            m_animationToImitate.RemoveAt(0);
        }

        private void RecordInfoToImitate()
        {
            m_positionToImitate.Add(m_toImitate.position);
            var scale = m_toImitate.localScale;
            scale.x *= -1;
            m_scaleToImitate.Add(scale);

            m_animationToImitate.Add(new AnimationParameterInfo(m_animatorToImitate, m_animationParametersData));
        }

        private void DrawLineConnection()
        {
            var yOffset = Vector3.up * m_lineYOffset;
            m_lineConnection.positionCount = m_positionToImitate.Count + 1;
            m_lineConnection.SetPosition(0, transform.position + yOffset);
            for (int i = 0; i < m_positionToImitate.Count; i++)
            {
                m_lineConnection.SetPosition(i + 1, m_positionToImitate[i] + yOffset);
            }
        }

        private void Start()
        {
            StartImitating(GameplaySystem.playerManager.player);
        }

        private void Update()
        {

            RecordInfoToImitate();

            if (m_isDelayed)
            {
                m_imitationDelayTimer -= GameplaySystem.time.deltaTime;
                if (m_imitationDelayTimer <= 0)
                {
                    m_isDelayed = false;
                }
            }
            else
            {
                UpdateImitation();
            }

            DrawLineConnection();
        }


    }
}