using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundednessHandle : MonoBehaviour, IComplexCharacterModule
    {
        private IGroundednessState m_state;
        private CharacterPhysics2D m_physics;
        private Animator m_animator;
        private string m_midAirParamater;
        private string m_speedYParamater;
        private bool m_isInMidAir;
        private Vector3 m_endpos;
        private Vector3 m_startpos;
        private RaySensor m_platformSlopeSensor;
        private GameObject m_modelObj;


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
            m_landHandle.Execute();
            LandExecuted?.Invoke(this, EventActionArgs.Empty);
            m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
            m_fallHandle.ResetValue();
            SetValuesToGround();
            
        }

        private void checkAngle()
        {
            m_platformSlopeSensor.Cast();
            m_startpos = transform.eulerAngles;
            m_modelObj = GameObject.Find("ZeeModel");
            var hits = m_platformSlopeSensor.GetHits();
            float slopeAngle = Vector2.Angle(hits[0].normal, Vector2.up);
            
            if (slopeAngle != 90 && slopeAngle < 40)
            {
                
                m_modelObj.transform.eulerAngles = new Vector3(transform.root.rotation.x, transform.root.rotation.y, slopeAngle);
            }
            //m_endpos = new Vector3(m_modelObj.transform.rotation.x, m_modelObj.transform.rotation.y, slopeAngle);
            //m_modelObj.transform.eulerAngles = Vector3.Lerp(m_startpos, m_endpos, Time.deltaTime * 80);
        }


        public void Initialize(ComplexCharacterInfo info)
        {
            m_physics = info.physics;
            m_state = info.state;
            m_animator = info.animator;
            m_midAirParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsMidAir);
            m_speedYParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_platformSlopeSensor = info.GetSensor(PlayerSensorList.SensorType.DecSlope
                );
            m_fallHandle.Initialize(info);
            m_landHandle.Initialize(info);
            m_skillRequester = info.skillResetRequester;
        }

        private void Start()
        {
            m_state.isGrounded = m_physics.onWalkableGround;
        }

        public void FixedUpdate()
        {
            if (m_state.isGrounded)
            {
                m_state.isFalling = false;
                m_state.isGrounded = m_physics.onWalkableGround;
                if (m_isInMidAir)
                {
                    SetValuesToGround();
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
            else
            {
               
                
                if (m_isInMidAir == false)
                {
                    m_isInMidAir = true;
                    m_animator.SetBool(m_midAirParamater, true);
                }
                var isFalling = m_fallHandle.isFalling(m_physics);
                if (isFalling)
                {
                    if (m_state.isFalling)
                    {
                        m_fallHandle.Execute(Time.deltaTime);
                    }
                    else
                    {
                        m_fallHandle.StartFall();
                    }
                    checkAngle();
                }
                else
                {
                    m_animator.SetInteger(m_speedYParamater, m_physics.velocity.y > m_startPeakVelocity ? 2 : 1);
                    m_physics.gravity.gravityScale = m_midAirGravity;
                    m_state.isFalling = false;
                }

                var hasLanded = m_physics.onWalkableGround;
                if (hasLanded)
                {
                    
                    CallLand();
                }
                m_landHandle.RecordVelocity();
            }
        }

        private void SetValuesToGround()
        {
            
            m_isInMidAir = false;
            m_animator.SetBool(m_midAirParamater, false);
            m_physics.gravity.gravityScale = m_groundGravity;
            m_state.isGrounded = true;
            Debug.Log("Is in mid air: " + m_isInMidAir + " ground Gravity: " + m_groundGravity + " Stade grounded: " + m_state.isGrounded);
        }

        private void OnDisable()
        {
            m_landHandle.SetRecordedVelocity(Vector2.zero);
        }
    }

}