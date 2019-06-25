using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Inputs;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class WallStick : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private float m_stickPositionOffset;
        [SerializeField]
        private float m_stickDuration;
        [SerializeField]
        private CountdownTimer m_stickTimer;
        [SerializeField]
        [MinValue(0.1)]
        private float m_slideSpeed;
        private bool m_isSliding;

        private PlayerInput m_input;
        private CharacterPhysics2D m_physics;
        private CharacterColliders m_colliders;
        private RaySensor m_sensor;
        private RaySensor m_groundHeightSensor;
        private Skills m_skills;
        private IFacing m_playerFacing;
       

        private IIsolatedTime m_time;
        private IWallStickState m_wallStickState;
        private IWallJumpState m_wallJumpState;
        private IWallStickModifier m_modifier;

        public void Initialize(IPlayerModules player)
        {
            m_physics = player.physics;
            m_time = player.isolatedObject;
            m_wallStickState = player.characterState;
            m_wallJumpState = player.characterState;
            m_sensor = player.sensors.wallStickSensor;
            m_groundHeightSensor = player.sensors.groundHeightSensor;
            m_colliders = player.colliders;
            m_modifier = player.modifiers;
            m_input = m_physics.GetComponent<PlayerInput>();
            m_skills = player.skills;
            m_playerFacing = player;
        }

        public void ConnectEvents()
        {
            var wallStickController = GetComponentInParent<IWallStickController>();
            wallStickController.WallStickCall += OnWallStickCall;
            wallStickController.UpdateCall += OnUpdateCall;
            GetComponentInParent<ILandController>().LandCall += OnLandCall;
        }

        public void HandleWallStick()
        {
            if (m_isSliding)
            {
                 
                m_physics.simulateGravity = false;
                m_physics.SetVelocity(Vector2.down);
                Debug.Log("slide condition true");
            }

            else
            {
                
                   
                    m_stickTimer.Tick(m_time.deltaTime);
                    m_physics.simulateGravity = true;
                    Debug.Log("is sticking");
                
            }
        }

        private void StickToWall()
        {
            m_wallJumpState.isWallJumping = false;
            m_stickTimer.SetStartTime(m_stickDuration * m_modifier.stickDuration);
            m_stickTimer.Reset();
            m_physics.SetVelocity(Vector2.zero);
            m_physics.simulateGravity = false;
            m_isSliding = false;
            m_wallStickState.isSlidingToWall = false;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_isSliding = true;
            m_wallStickState.isSlidingToWall = true;
            m_physics.simulateGravity = true;
        }

        private void OnWallStickCall(object sender, EventActionArgs eventArgs)
        {
            if (m_skills.IsEnabled(MovementSkill.WallJump))
                HandleWallStick();
        }

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            m_isSliding = false;
            m_wallStickState.isSlidingToWall = false;
            m_wallJumpState.isWallJumping = false;
            m_wallStickState.isStickingToWall = false;
            m_physics.SetVelocity(Vector2.zero);
            m_physics.simulateGravity = true;
        }

        private void OnUpdateCall(object sender, ControllerEventArgs eventArgs)
        {
            if (m_skills.IsEnabled(MovementSkill.WallJump))
            {
                if (m_wallStickState.isMoving && m_wallStickState.isDroppingFromPlatform == false && m_colliders.AreCollidersIntersecting() == false)
                {
                    if (m_physics.velocity.y <= 0)
                    {
                        m_sensor.Cast();

                        if (m_sensor.allRaysDetecting)
                        {
                            var hit = m_sensor.GetValidHits()[0];
                            if (m_wallStickState.isStickingToWall && m_stickTimer.time > -1 &&  eventArgs.input.direction.isLeftHeld && m_playerFacing.currentFacingDirection == HorizontalDirection.Left )//
                            {
                                m_physics.SetVelocity(Vector2.zero);
                                m_physics.simulateGravity = false;
                            }
                            else
                             if (m_wallStickState.isStickingToWall && m_stickTimer.time > -1 && eventArgs.input.direction.isRightHeld && m_playerFacing.currentFacingDirection == HorizontalDirection.Right)//
                            {
                                m_physics.SetVelocity(Vector2.zero);
                                m_physics.simulateGravity = false;
                            }
                            else
                            {
                                Debug.Log("Trigger force");
                                m_physics.simulateGravity = true;
                                m_physics.SetVelocity(Vector2.down);
                                m_wallStickState.isSlidingToWall = true;
                                m_physics.AddForce(new Vector2(0, -1000));
                               
                            }
                          

                            if (hit.collider.CompareTag("Droppable") == false)
                            {
                                m_groundHeightSensor.Cast();
                                if (m_groundHeightSensor.isDetecting == false)
                                { 
                                    AttachToWall(hit);
                                    if (m_wallStickState.isStickingToWall == false)
                                    {
                                       
                                            StickToWall();
                                       
                                    }
                                }
                                else
                                    m_physics.simulateGravity = true;
                                    
                            }
                        }
                        else if (m_wallStickState.isStickingToWall)
                        {
                            m_isSliding = false;
                            m_wallStickState.isSlidingToWall = false;
                            m_physics.simulateGravity = true;
                        }

                        m_wallStickState.isStickingToWall = m_sensor.allRaysDetecting;
                    }
                    else
                    {
                        m_wallStickState.isStickingToWall = false;
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

        private void Awake()
        {
            m_stickTimer.CountdownEnd += OnCountdownEnd;
        }

        private void Start()
        {
            m_stickTimer.Reset();
        }

#if UNITY_EDITOR
        public void Initialize(float positionOffset, float stickDuration, float slideSpeed)
        {
            m_stickPositionOffset = positionOffset;
            m_stickTimer = new CountdownTimer(stickDuration);
            m_slideSpeed = slideSpeed;
        }
#endif
    }
}