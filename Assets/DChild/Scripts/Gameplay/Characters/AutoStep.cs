using DChild.Gameplay.Characters.Players;
using DChild.Inputs;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class AutoStep : MonoBehaviour
    {
        [SerializeField]
        [TabGroup("Sensors")]
        private RaySensor m_sensor;
        [SerializeField]
        [TabGroup("Step Config")]
        private float m_tolerableHeight;
        [SerializeField]
        [TabGroup("Step Config")]
        private float m_stepDistance;
        [SerializeField]
        [TabGroup("Step Config")]
        private float m_stepSpeed;

        [SerializeField]
        [MinValue(1)]
        private int m_hitCount;

        [SerializeField]
        private LayerMask m_layer;

        private bool m_facingRight;
        private bool m_stepUp;
        private RaycastHit2D[] m_hits;
        private List<GameObject> m_detectedObjects;
        private Vector3 m_targetPos;
        private CharacterPhysics2D m_characterPhysics2D;
        private PlayerInput m_playerInput;
        private Player m_player;
        public bool isAutoStepping { get; set; }

        public bool Enable() => this.enabled = true;
        public bool Disable() => this.enabled = false;

        private void DoStep()
        {
            if (m_stepUp)
            {
                DisableGravity();
                m_player.transform.position = Vector3.Lerp(m_player.transform.position, m_targetPos, m_stepSpeed * Time.deltaTime);
                if (m_facingRight)
                {
                    if (m_player.transform.position.x >= m_targetPos.x) EnableGravity();
                }
                else
                {
                    if (m_player.transform.position.x <= m_targetPos.x) EnableGravity();
                }
            }
        }

        private void CastRays()
        {
            if (enabled)
            {
                m_detectedObjects.Clear();
                m_sensor.Cast();
                m_hits = m_sensor.GetUniqueHits();
                for (int i = 0; i < m_hits.Length; i++)
                {
                    var detectedObject = m_hits[i].collider.gameObject;
                    if (m_detectedObjects.Contains(detectedObject) == false)
                    {
                        m_detectedObjects.Add(detectedObject);
                    }
                } 
            }
        }


        private void DisableGravity()
        {
            m_characterPhysics2D.simulateGravity = false;
            GetComponentInParent<Rigidbody2D>().gravityScale = 0;
            isAutoStepping = true;
        }

        private void EnableGravity()
        {
            m_stepUp = false;
            m_characterPhysics2D.simulateGravity = true;
            GetComponentInParent<Rigidbody2D>().gravityScale = 1;
        }


        private void Update()
        {
            var HorizontalInput = Input.GetAxis("Horizontal");

            if (m_characterPhysics2D.velocity.x != 0)
            {
                CastRays();
                m_facingRight = HorizontalInput > 0;
                if (m_sensor.isDetecting)
                {
                    var sensor = m_detectedObjects[0].GetComponent<Collider2D>();
                    if (HorizontalInput != 0)
                    {
                        if (!m_stepUp)
                        {
                            var stepHeight = sensor.bounds.extents.y;
                            if (stepHeight <= m_tolerableHeight)
                            {
                                m_targetPos = m_player.transform.position;

                                m_targetPos.y += (sensor.bounds.extents.y * 2);

                                m_stepUp = true;

                                if (m_facingRight) m_targetPos.x += m_stepDistance;
                                else m_targetPos.x -= m_stepDistance;
                            }
                        }
                    }
                    else EnableGravity();
                }
                else
                {
                    if (m_playerInput.isJumpPressed || m_playerInput.isJumpHeld || HorizontalInput == 0) EnableGravity();
                }
                DoStep();
            }
        }

        private void Awake()
        {
            m_characterPhysics2D = GetComponentInParent<CharacterPhysics2D>();
            m_playerInput = GetComponentInParent<PlayerInput>();
            m_player = GetComponentInParent<Player>();
            m_hits = new RaycastHit2D[m_hitCount];
            m_detectedObjects = new List<GameObject>();
            Raycaster.SetLayerMask(m_layer);
        }
    } 
}