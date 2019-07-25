using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;
using System;
using Spine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class PlayerLandHandler : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private float m_velocityYLimit;
        [SerializeField]
        private float m_durationLimit;

        private Animator m_animator;
        private CountupTimer m_countupTimer;
        private PlayerAnimation m_animation;
        private CharacterPhysics2D m_physics;

        private float m_fallVelocityY;
        private bool m_doHardLanding;

        private IIsolatedTime m_time;
        private IFacing m_facing;

        private IBehaviourState m_behaviourState;
        private IPlayerAnimationState m_animationState;
        private IHighJumpState m_highJumpState;
        private IPlayerState m_characterState;

        private Action<PlayerAnimation> HandleLand = delegate { };

        public void Initialize(IPlayerModules player)
        {
            m_facing = player;
            m_physics = player.physics;
            m_time = player.isolatedObject;
            m_animation = player.animation;
            m_highJumpState = player.characterState;
            m_characterState = player.characterState;
            m_behaviourState = player.characterState;
            m_animationState = player.animationState;
            m_animator = player.animation.GetComponentInChildren<Animator>();
        }

        public void SetLandHandler(Action<PlayerAnimation> handle)
        {
            HandleLand = handle == null ? LandCall : handle;
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IFallController>().FallUpdate += OnFallUpdate;
        }

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            if (m_animationState.isHardLanding)
            {
                m_behaviourState.waitForBehaviour = true;
            }

            m_highJumpState.canHighJump = false;
            HandleLand(m_animation);
        }

        private void LandCall(PlayerAnimation animation)
        {
            if (m_animationState.isHardLanding)
            {
                m_animation.DoHardLanding(m_facing.currentFacingDirection);
            }

            else
            {
                if (m_animationState.isFromFall || m_animationState.hasJumped || m_animationState.hasWallSticked)
                {
                    m_animationState.isLanding = true;

                    if (m_animationState.isFallingFromWallJump)
                    {
                        m_animation.DoMovingLand(m_facing.currentFacingDirection);
                    }

                    else if (m_characterState.isMoving)
                    {
                        m_animation.DoFallToJog(m_facing.currentFacingDirection);
                        m_animationState.isFallingToJog = true;
                    }

                    else
                    {
                        if (m_animationState.isShortJumping)
                        {
                            m_animation.DoStaticLand(m_facing.currentFacingDirection);
                        }

                        else
                        {

                            m_animation.DoMovingLand(m_facing.currentFacingDirection);
                        }
                    }

                    m_animationState.isFallingFromWallJump = false;
                }
            }
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            m_countupTimer.Reset();
            m_animationState.ResetAnimations();
            m_behaviourState.waitForBehaviour = false;
        }

        private void OnFallUpdate(object sender, EventActionArgs eventArgs)
        {
            if (m_characterState.isFalling)
                m_countupTimer.Tick(m_time.deltaTime);

            if (m_physics.velocity.y != 0)
            {
                m_fallVelocityY = m_physics.velocity.y;
            }

            if (m_fallVelocityY <= m_velocityYLimit)
            {
                if (m_countupTimer.time > m_durationLimit)
                {
                    m_animationState.transitionToFall2 = true;
                    m_animationState.isHardLanding = true;

                }
            }
        }

        private void Update()
        {
            if (m_animation.skeletonAnimation.AnimationName == "Jump1_Landing_Right2")
            {
                if (m_animation.animationState.GetCurrent(0).AnimationTime >= 0.3)
                {
                    m_animation.DoStandIdle(m_facing.currentFacingDirection);
                }
            }

            else if (m_animation.skeletonAnimation.AnimationName == "Jump1_Landing_Left2")
            {
                if (m_animation.animationState.GetCurrent(0).AnimationTime >= 0.3)
                {
                    m_animation.DoStandIdle(m_facing.currentFacingDirection);
                }
            }
        }

        private void Awake()
        {
            GetComponentInParent<ILandController>().LandCall += OnLandCall;
            HandleLand = LandCall;
        }

        private void Start()
        {
            m_countupTimer = new CountupTimer();
            m_animation.animationState.Complete += OnComplete;
        }

#if UNITY_EDITOR
        public void Initialize(float yLimit, float duration)
        {
            m_velocityYLimit = yLimit;
            m_durationLimit = duration;
        }
#endif
    }

}