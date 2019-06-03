using System;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using Spine.Unity.Modules;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public abstract class Dash : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        protected SkeletonGhost m_ghosting;

        [SerializeField]
        [MinValue(0.1)]
        protected float m_power;
        [SerializeField]
        protected CountdownTimer m_duration;
        protected CharacterPhysics2D m_physics;
        protected IIsolatedTime m_time;
        protected IFacing m_facing;
        protected Vector2 m_direction;

        protected IDashState m_state;
        protected IDashModifier m_modifier;

        public event EventAction<EventActionArgs> DashStart;
        public event EventAction<EventActionArgs> DashEnd;

        public virtual void Initialize(IPlayerModules player)
        {
            m_physics = player.physics;
            m_time = player.isolatedObject;
            m_facing = player;
            m_state = player.characterState;
            m_modifier = player.modifiers;
        }

        protected void CallDashStart() => DashStart?.Invoke(this, EventActionArgs.Empty);
        protected void CallDashEnd() => DashEnd?.Invoke(this, EventActionArgs.Empty);

        protected abstract void OnDashDurationEnd(object sender, EventActionArgs eventArgs);

        protected virtual void Awake()
        {
            m_duration.CountdownEnd += OnDashDurationEnd;
        }

        protected virtual void Update()
        {
            m_duration.Tick(m_time.deltaTime);
        }

#if UNITY_EDITOR
        public void Initialize(float power, float duration)
        {
            m_power = power;
            m_duration = new CountdownTimer(duration);
        }
#endif
    }
}