using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Holysoft;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class QuickSand : MonoBehaviour
    {
        [SerializeField]
        private Transform m_surface;
        [SerializeField]
        private float m_Sinkspeed;
        [SerializeField]
        private float m_SlowValue;
        [SerializeField, HorizontalGroup("Start"), ShowIf("m_surface")]
        private Vector2 m_StartPosition;
        [SerializeField, HorizontalGroup("End"), ShowIf("m_surface")]
        private Vector2 m_EndPosition;
        private Vector2 m_start;
        private Vector2 m_destination;
        [SerializeField]
        private bool m_resurface = false;
        private float m_surfacespeed = 3f;

        private void OnTriggerEnter2D(Collider2D collision)
        {

            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null)
            {
                playerObject.owner.modifiers.Add(PlayerModifier.MoveSpeed, -m_SlowValue);
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null)
            {
                SetMoveValues(m_surface.localPosition, m_EndPosition);
                m_surface.localPosition = Vector3.MoveTowards(m_start, m_destination, m_Sinkspeed);

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {

            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null)
            {
                m_resurface = true;

                playerObject.owner.modifiers.Add(PlayerModifier.MoveSpeed, m_SlowValue);
            }
        }
        private void SetMoveValues(Vector2 start, Vector2 destination)
        {
            m_start = start;
            m_destination = destination;
        }
        private Vector2 RoundVectorValuesTo(uint decimalPlace, Vector2 vector2)
        {
            return new Vector2(MathfExt.RoundDecimalTo(decimalPlace, vector2.x), MathfExt.RoundDecimalTo(decimalPlace, vector2.y));
        }
#if UNITY_EDITOR
        [ResponsiveButtonGroup("Start/Button"), Button("Use Current"), ShowIf("m_surface")]
        private void UseCurrentForStartPosition()
        {
            m_StartPosition = m_surface.localPosition;
        }

        [ResponsiveButtonGroup("End/Button"), Button("Use Current"), ShowIf("m_surface")]
        private void UseCurrentForEndPosition()
        {
            m_EndPosition = m_surface.localPosition;
        }
#endif
        public void FixedUpdate()
        {
            if (m_resurface == true)
            {
                SetMoveValues(m_surface.localPosition, m_StartPosition);
                m_surface.localPosition = Vector3.MoveTowards(m_start, m_destination, m_surfacespeed * GameplaySystem.time.fixedDeltaTime);
                if (RoundVectorValuesTo(2, m_surface.localPosition) == RoundVectorValuesTo(2, m_StartPosition))
                {
                    m_resurface = false;
                }
            }

        }
    }
}

