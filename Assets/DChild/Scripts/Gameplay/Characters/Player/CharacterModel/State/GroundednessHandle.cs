using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundednessHandle : MonoBehaviour, IComplexCharacterModule
    {
        private IGroundednessState m_state;
        private IHighJumpState m_jumpState;
        private IDoubleJumpState m_doubleJumpState;
        private IWallStickState m_wallStickState;
        private CharacterPhysics2D m_physics;
        private RaySensor m_groundSensor;
        private RaySensor m_slopeSensor;
        private Animator m_animator;
        private string m_midAirParamater;
        private string m_speedYParamater;
        private bool m_isInMidAir;
        private bool m_canDoubleJump;

        [SerializeField]
        private float m_groundGravity;
        [SerializeField]
        private float m_midAirGravity;
        [SerializeField, MinValue(0.1)]
        private float m_startPeakVelocity;

        [SerializeField]
        private FallHandle m_fallHandle;
        [SerializeField]
        private LandHandle m_landHandle;

        private SkillResetRequester m_skillRequester;
        public event EventAction<EventActionArgs> LandExecuted;

        public void CallLand()
        {
            if (m_physics.velocity.x == 0)
            {
                m_landHandle.Execute(true);

            }
            LandExecuted?.Invoke(this, EventActionArgs.Empty);
            m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
            SetValuesToGround();

        }

        public void CallLandJog() {
            m_jumpState.hasJumped = false;
            m_doubleJumpState.canDoubleJump = true;
            m_state.isGrounded = true;

        }

        public void ResetAnimationParameters()
        {
            m_fallHandle.ResetValue();
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_physics = info.physics;
            m_state = info.state;
            m_wallStickState = info.state;
            m_animator = info.animator;

            m_midAirParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsMidAir);
            m_speedYParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_groundSensor = info.GetSensor(PlayerSensorList.SensorType.Ground);
            m_slopeSensor = info.GetSensor(PlayerSensorList.SensorType.Slope);


            m_fallHandle.Initialize(info);
            m_landHandle.Initialize(info);
            m_skillRequester = info.skillResetRequester;
        }

        public void Initialize()
        {
            m_state.isGrounded = m_physics.onWalkableGround;
        }

        public void HandleLand()
        {
            m_slopeSensor.Cast();

            var hasLanded = m_physics.onWalkableGround;
            var incontactwithground = m_physics.inContactWithGround;
            float slopeAngle = Vector3.Angle(Vector3.up, m_slopeSensor.GetHits()[0].normal);

            if (hasLanded == true || m_slopeSensor.isDetecting == true && slopeAngle < 35.0f)
            {
             
                m_animator.SetBool(m_midAirParamater, false);
                if(m_wallStickState.isSlidingToWall == false && m_wallStickState.isStickingToWall == false)
                {
                    Debug.Log("call land here");
                    CallLand();
                }
                    
               
            }
           

            m_landHandle.RecordVelocity();
        }

        public void HandleMidAir()
        {
            if (m_isInMidAir == false)
            {
                m_isInMidAir = true;
                m_animator.SetBool(m_midAirParamater, true);
            }


            //Pontz

            m_groundSensor.Cast();
           
                var isFalling = m_fallHandle.isFalling(m_physics);

                if (isFalling && !m_groundSensor.isDetecting)
                {

                    if (m_state.isFalling)
                    {
                        m_fallHandle.Execute(Time.deltaTime);
                    }
                    else
                    {

                        m_fallHandle.StartFall();

                    }
                }
                else
                {

                    m_animator.SetInteger(m_speedYParamater, m_physics.velocity.y > m_startPeakVelocity ? 2 : 1);
                    m_physics.gravity.gravityScale = m_midAirGravity;
                    m_state.isFalling = false;
                }
            
           
        }

        public void HandleGround()
        {
            m_state.isFalling = false;
           
            m_state.isGrounded = m_physics.onWalkableGround;
            if (m_isInMidAir)
            {
                SetValuesToGround();
                LandExecuted?.Invoke(this, EventActionArgs.Empty);
                m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
            }
            if (m_physics.inContactWithGround)
            {
                m_physics.gravity.gravityScale = m_groundGravity;
            }
            else
            {
                m_physics.gravity.gravityScale = m_midAirGravity;
            }
        }

        private void SetValuesToGround()
        {
            m_isInMidAir = false;
            m_animator.SetBool(m_midAirParamater, false);
            m_physics.gravity.gravityScale = m_groundGravity;
            m_state.isGrounded = true;
        }

        private void OnDisable()
        {
            m_landHandle.SetRecordedVelocity(Vector2.zero);
        }
    }

}