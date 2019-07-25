using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Inputs;
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
        [SerializeField]
        private CountdownTimer m_stickTimer;
        [SerializeField, MinValue(0.1)]
        private float m_slideSpeed;
        private bool m_isSliding;
 
        private CharacterPhysics2D m_physics;
        private CharacterColliders m_colliders;
        private RaySensor m_sensor;
        private RaySensor m_groundHeightSensor;

        private IIsolatedTime m_time;
        private IWallStickState m_wallStickState;

        #region Initialization
        public void ConnectTo(IMainController controller)
        {
            var wallStickController = controller.GetSubController<IWallStickController>();
            wallStickController.WallStickCall += OnWallStickCall;
            wallStickController.WallSlideCall += OnWallSlideCall;
            wallStickController.UpdateCall += OnUpdateCall;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_physics = info.physics;
            m_time = info.character.isolatedObject;
            m_wallStickState = info.state;
            m_sensor = info.GetSensor(PlayerSensorList.SensorType.WallStick);
            m_groundHeightSensor = info.GetSensor(PlayerSensorList.SensorType.WallStickHeight);
            m_colliders = info.character.colliders;
            info.groundednessHandle.LandExecuted += OnLandCall;
        } 
        #endregion

        public void HandleWallStick()
        {
            if (m_isSliding)
            {
                m_physics.SetVelocity(Vector2.down);
                m_physics.AddForce(new Vector2(0, -250));
            }
            else
            {
                m_stickTimer.Tick(m_time.deltaTime);
            }
        }

        #region WallStick Only
        private void AttemptToWallStick()
        {
            //if (m_skills.IsEnabled(PrimarySkill.WallJump) && m_wallStickState.isStickingToWall == false)
            {
                m_groundHeightSensor.Cast();
                if (m_groundHeightSensor.isDetecting == false)
                {
                    if (m_physics.velocity.y <= 0)
                    {
                        // Be more specific on wallstick is moving
                        if (m_wallStickState.isMoving && m_wallStickState.isDroppingFromPlatform == false && m_colliders.AreCollidersIntersecting() == false)
                        {
                            m_sensor.Cast();
                            if (m_sensor.allRaysDetecting)
                            {
                                var hit = m_sensor.GetValidHits()[0];
                                if (hit.collider.CompareTag("Droppable") == false)
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
            m_stickTimer.SetStartTime(m_stickDuration);
            m_stickTimer.Reset();
            m_physics.SetVelocity(Vector2.zero);
            m_physics.simulateGravity = false;
            m_isSliding = false;
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
        private void DoWallSlide()
        {
            m_physics.simulateGravity = true;
            m_physics.SetVelocity(Vector2.down);
            m_wallStickState.isSlidingToWall = true;
            m_stickTimer.EndTime(true);
        }

        private void StartWallSlide()
        {
            m_isSliding = true;
            m_wallStickState.isSlidingToWall = true;
            m_physics.simulateGravity = true;
            m_stickTimer.EndTime(false);
        }

        private void OnWallSlideCall(object sender, EventActionArgs eventArgs)
        {
            StartWallSlide();
        } 
        #endregion

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            CancelWallStick();
            m_physics.SetVelocity(Vector2.zero);
        }

        private void CancelWallStick()
        {
            m_isSliding = false;
            m_wallStickState.isSlidingToWall = false;
            m_wallStickState.isStickingToWall = false;
            m_physics.simulateGravity = true;
        }

        private void OnUpdateCall(object sender, ControllerEventArgs eventArgs)
        {
            AttemptToWallStick();
        }

        private void Awake()
        {
            m_stickTimer.CountdownEnd += OnWallStickEnd;
        }

        private void Start()
        {
            m_stickTimer.Reset();
        }
    }
}