using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;


namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class FallAssist : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private CountdownTimer m_duration;
        [SerializeField]
        private float m_FallGravity;
        private float m_defaultGravity;
        private bool m_stopAssist;

        private CharacterPhysics2D m_character;

        private IPlacementState m_state;
        private IIsolatedTime m_time;

        public void Initialize(IPlayerModules player)
        {
            m_state = player.characterState;
            m_time = player.isolatedObject;
            m_character = player.physics;
            m_defaultGravity = m_character.gravity.gravityScale;
        }

        private void Update()
        {
            if (m_state.isFalling)
            {
                if (m_stopAssist)
                {
                    m_character.gravity.gravityScale = m_defaultGravity;
                }
                else
                {
                    m_duration.Tick(m_time.deltaTime);
                    m_character.gravity.gravityScale = m_FallGravity;
                }
            }
            else if (m_state.hasLanded)
            {
                m_duration.Reset();
                m_stopAssist = false;
                m_character.gravity.gravityScale = m_defaultGravity;
            }
        }

        private void Awake()
        {
            m_duration.CountdownEnd += OnDurationEnd;
        }

        private void OnDurationEnd(object sender, EventActionArgs eventArgs)
        {
            m_stopAssist = true;
            m_duration.Reset();
        }

#if UNITY_EDITOR
        public void Initialize(float duration, float fallGravity)
        {
            m_duration = new CountdownTimer(duration);
            m_FallGravity = fallGravity;
        }
#endif
    }
}