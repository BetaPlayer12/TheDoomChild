using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ParticleFX : FX
    {
        public event EventAction<EventActionArgs> FXDisabled;

        private enum State
        {
            Stop,
            Play,
            Pause
        }

        [SerializeField, HideInInspector]
        private ParticleSystem[] m_particleSystems;
        [SerializeField]
        private bool m_playOnAwake;
        [SerializeField]
        private bool m_baseFlipped;
        private State m_state;

        private ParticleFXComponent[] m_components;
#if UNITY_EDITOR
        [SerializeField]
        private bool m_poolOnStop;
#endif

        public int count => m_particleSystems.Length;
        public bool isEmmiting => m_particleSystems[0].isEmitting;

        public ParticleSystem GetParticle(int i) => m_particleSystems[i];

        [Button("Play")]
        public override void Play()
        {
            for (int i = 0; i < m_particleSystems.Length; i++)
            {
                m_particleSystems[i].Play();
            }

            if (m_state == State.Pause)
            {
                for (int i = 0; i < m_components.Length; i++)
                {
                    m_components[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < m_components.Length; i++)
                {
                    m_components[i].Reset();
                }
            }
            m_state = State.Play;
        }

        [Button("Stop")]
        public override void Stop()
        {
            if (m_state != State.Stop)
            {
                for (int i = 0; i < m_particleSystems.Length; i++)
                {
                    m_particleSystems[i].Stop();
                }

                for (int i = 0; i < m_components.Length; i++)
                {
                    m_components[i].Stop();
                }
                m_state = State.Stop;
            }
        }

        [Button("Pauses")]
        public override void Pause()
        {
            if (m_state != State.Pause)
            {
                for (int i = 0; i < m_particleSystems.Length; i++)
                {
                    m_particleSystems[i].Pause();
                }

                for (int i = 0; i < m_components.Length; i++)
                {
                    m_components[i].SetActive(false);
                }
                m_state = State.Pause;
            }
        }

        public void DetachChildren()
        {
            for (int i = 0; i < m_particleSystems.Length; i++)
            {
                m_particleSystems[i].transform.parent = null;
            }

            Destroy(this);
        }

        private void OnParticleSystemStopped()
        {
            m_state = State.Stop;
            CallFXDone();
            CallPoolRequest();
        }

        private void Awake()
        {
            m_state = m_playOnAwake ? State.Play : State.Stop;
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            m_components = GetComponentsInChildren<ParticleFXComponent>();
            for (int i = 0; i < m_components.Length; i++)
            {
                m_components[i].Initialize();
            }
        }

        private void OnValidate()
        {
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; i++)
            {
                var main = m_particleSystems[i].main;
                if (main.playOnAwake != m_playOnAwake)
                {
                    main.playOnAwake = m_playOnAwake;
                }
#if UNITY_EDITOR
                if (m_poolOnStop && main.stopAction != ParticleSystemStopAction.Callback)
                {
                    main.stopAction = ParticleSystemStopAction.Callback;
                }
#endif
            }
        }

        public override void SetFacing(HorizontalDirection horizontalDirection)
        {
            var signDirection = (int)horizontalDirection * (m_baseFlipped ? -1 : 1);
            var scale = transform.localScale;
            scale.x *= signDirection;
            transform.localScale = scale;
        }
    }
}