using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    [RequireComponent(typeof(LineRenderer))]
    public class HookDash : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        [MinValue(0.1)]
        private float m_power;
        [SerializeField]
        private float m_delay;

        private IWhipGrapple m_state;

        private CharacterPhysics2D m_character;
        private LineRenderer m_hookLine;
        private float m_count;

        public void Initialize(IPlayerModules player)
        {
            m_state = player.characterState;
            m_character = player.physics;
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
                m_character.simulateGravity = false;
            }

            else
            {
                m_character.simulateGravity = true;
            }
        }

        private void HandleHookDash()
        {
            if (m_count <= 0)
            {
                m_state.isHookDashing = false;
                m_count = m_delay;

                var targetPosition = GetMousePosition();
                targetPosition.z = m_character.transform.position.z;
                m_character.transform.position = Vector3.Lerp(m_character.transform.position, targetPosition, m_power);


                m_hookLine.SetPosition(0, m_character.transform.position);
                m_hookLine.SetPosition(1, m_character.transform.position);
            }
            else
            {
                m_count = m_count - Time.deltaTime;
            }
        }

        private void CreateHookLine()
        {
            m_hookLine.SetPosition(0, m_character.transform.position);
            m_hookLine.SetPosition(1, GetMousePosition());
        }

        private Vector3 GetMousePosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_character.transform.position.z));
            return mousePosition;
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