using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class WallStick : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField]
        private float m_stickPositionOffset;
        [SerializeField]
        private float m_stickDuration;
        private CountdownTimer m_stickTimer;
        [SerializeField, MinValue(0.1)]
        private float m_slideSpeed;
        private bool m_isSliding;

        private CharacterPhysics2D m_physics;
        private CharacterColliders m_colliders;
        private RaySensor m_wallSensor;
        private RaySensor m_groundHeightSensor;
        private RaySensor m_platformSensor;
        private GroundednessHandle m_groundednessHandle;

        private Animator m_animator;
        private string m_speedYParameter;
        private string m_wallStickTriggerParameter;
        private string m_wallStickParameter;
        private string m_wallSlideParameter;

        private IIsolatedTime m_time;
        private IWallStickState m_wallStickState;

        #region Initialization
        public void ConnectTo(IMainController controller)
        {
            controller.ControllerDisabled += OnControllerDisablled;
        }

        private void OnControllerDisablled(object sender, EventActionArgs eventArgs)
        {
            CancelWallStick();
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_physics = info.physics;
            m_time = info.character.isolatedObject;
            m_wallStickState = info.state;
            m_wallSensor = info.GetSensor(PlayerSensorList.SensorType.WallStick);
            m_groundHeightSensor = info.GetSensor(PlayerSensorList.SensorType.GroundHeight);
            m_platformSensor = info.GetSensor(PlayerSensorList.SensorType.Platform);
            m_colliders = info.character.colliders;
            m_groundednessHandle = info.groundednessHandle;

            m_animator = info.animator;
            m_speedYParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_wallStickTriggerParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WallStickTrigger);
            m_wallStickParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WallStick);
            m_wallSlideParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WallSlide);
        }
        #endregion

        public void HandleWallStick()
        {
            if (m_isSliding)
            {
                m_physics.SetVelocity(Vector2.down * m_slideSpeed);

                m_wallSensor.Cast();
                if (m_wallSensor.isDetecting == false || m_physics.inContactWithGround)
                {
                    CancelWallStick();
                    m_groundednessHandle.CallLand();
                }
            }
            else
            {
                m_stickTimer.Tick(m_time.deltaTime);
                m_physics.SetVelocity(Vector2.zero);
            }
        }

        #region WallStick Only
        public void AttemptToWallStick()
        {
            if (m_wallStickState.isStickingToWall == false)
            {
                if (m_physics.velocity.y <= 0)
                {
                    // Be more specific on wallstick is moving
                    if (m_wallStickState.isMoving && m_colliders.AreCollidersIntersecting() == false)
                    {
                        m_wallSensor.Cast();
                        if (m_wallSensor.allRaysDetecting)
                        {
                            var hit = m_wallSensor.GetValidHits()[0];
                            if (hit.collider.CompareTag("Droppable") == false)
                            {
                                m_groundHeightSensor.Cast();
                                if (m_groundHeightSensor.isDetecting == false)
                                {
                                    m_platformSensor.Cast();
                                    if (m_platformSensor.isDetecting == false)
                                    {
                                        AttachToWall(hit);
                                        StartStickToWall();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CancelWallStick()
        {
            m_isSliding = false;
            m_wallStickState.isSlidingToWall = false;
            m_wallStickState.isStickingToWall = false;
            m_animator.SetBool(m_wallStickParameter, false);
            m_animator.SetBool(m_wallSlideParameter, false);
            m_physics.SetVelocity(Vector2.zero);
            m_physics.simulateGravity = true;
            m_groundednessHandle.enabled = true;
        }

        private void AttachToWall(RaycastHit2D hit)
        {
            var hitpoint = hit.point;
            var currentPosition = m_physics.transform.position;
            if (currentPosition.x > hitpoint.x)
            {
                m_physics.transform.position = new Vector2(hitpoint.x + m_stickPositionOffset, currentPosition.y);
            }
            else
            {
                m_physics.transform.position = new Vector2(hitpoint.x - m_stickPositionOffset, currentPosition.y);
            }
        }

        private void StartStickToWall()
        {
            m_wallStickState.isStickingToWall = true;
            m_wallStickState.isSlidingToWall = false;
            m_wallStickState.isFalling = false;
            m_wallStickState.isMoving = false;
            m_stickTimer.SetStartTime(m_stickDuration);
            m_stickTimer.Reset();
            m_physics.SetVelocity(Vector2.zero);
            m_physics.simulateGravity = false;
            m_isSliding = false;


            m_animator.SetInteger(m_speedYParameter, 0);
            m_animator.SetTrigger(m_wallStickTriggerParameter);
            m_animator.SetBool(m_wallStickParameter, true);
            m_animator.SetBool(m_wallSlideParameter, false);
        }

        private void OnWallStickEnd(object sender, EventActionArgs eventArgs)
        {
            StartWallSlide();
        }

        private void OnWallStickCall(object sender, EventActionArgs eventArgs)
        {
            HandleWallStick();
        }
        #endregion

        #region WallSlide Only
        public void StartWallSlide()
        {
            m_isSliding = true;
            m_wallStickState.isSlidingToWall = true;
            m_groundednessHandle.enabled = false;
            m_physics.simulateGravity = true;
            m_stickTimer.EndTime(false);
            m_animator.SetBool(m_wallSlideParameter, true);
        }

        private void DoWallSlide()
        {
            m_physics.simulateGravity = true;
            m_physics.SetVelocity(Vector2.down);
            m_wallStickState.isSlidingToWall = true;
            m_stickTimer.EndTime(true);
        }
        #endregion

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            CancelWallStick();
            m_physics.SetVelocity(Vector2.zero);
        }

        private void Awake()
        {
            m_stickTimer = new CountdownTimer(m_stickDuration);
            m_stickTimer.CountdownEnd += OnWallStickEnd;
        }

        private void Start()
        {
            m_stickTimer.Reset();
        }
    }
}