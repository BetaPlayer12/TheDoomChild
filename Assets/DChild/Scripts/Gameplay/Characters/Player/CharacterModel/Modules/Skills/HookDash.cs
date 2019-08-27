using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    [RequireComponent(typeof(LineRenderer))]
    public class HookDash : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        [MinValue(0.1)]
        private float m_power;
        [SerializeField]
        private float m_delay;

        private IWhipGrapple m_state;

        private CharacterPhysics2D m_physics;
        private LineRenderer m_hookLine;
        private float m_count;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_physics = info.physics;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Interact"))
            {
                m_state.isHookDashing = true;
            }

            if (m_state.isHookDashing)
            {
                CreateHookLine();
                HandleHookDash();
                m_physics.simulateGravity = false;
            }

            else
            {
                m_physics.simulateGravity = true;
            }
        }

        private void HandleHookDash()
        {
            if (m_count <= 0)
            {
                m_state.isHookDashing = false;
                m_count = m_delay;

                var targetPosition = GetMousePosition();
                targetPosition.z = m_physics.transform.position.z;
                m_physics.transform.position = Vector3.Lerp(m_physics.transform.position, targetPosition, m_power);


                m_hookLine.SetPosition(0, m_physics.transform.position);
                m_hookLine.SetPosition(1, m_physics.transform.position);
            }
            else
            {
                m_count = m_count - Time.deltaTime;
            }
        }

        private void CreateHookLine()
        {
            m_hookLine.SetPosition(0, m_physics.transform.position);
            m_hookLine.SetPosition(1, GetMousePosition());
        }

        private Vector3 GetMousePosition()
        {
            //var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_character.transform.position.z));
            //return mousePosition;
            return Vector3.zero;
        }

        private void Start()
        {
            m_count = m_delay;
            m_hookLine = GetComponent<LineRenderer>();
            m_hookLine.widthMultiplier = 0.1f;
            m_hookLine.material = new Material(Shader.Find("Sprites/Default"));
        }

      
    }
}