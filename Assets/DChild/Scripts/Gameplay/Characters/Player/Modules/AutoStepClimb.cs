﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class AutoStepClimb : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_legForwardEnvironment;
        [SerializeField]
        private RaySensor m_destinationFinder;
        [SerializeField, MinValue(0)]
        private float m_distanceFromLegContactForDestination;

        private Rigidbody2D m_physics;
        private Character m_character;

        private Vector2 m_climbToHere;
        private RaycastHit2D[] m_hitBuffer;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_physics = info.rigidbody;
        }

        public bool CheckForStepClimbableSurface()
        {
            m_legForwardEnvironment.Cast();
            if (m_legForwardEnvironment.isDetecting)
            {
                m_hitBuffer = m_legForwardEnvironment.GetValidHits();
                var legEnvironmentContact = m_hitBuffer[0].point;
                var destinationPosition = m_destinationFinder.transform.position;
                destinationPosition.x = legEnvironmentContact.x + (m_distanceFromLegContactForDestination * (int)m_character.facing);
                m_destinationFinder.transform.position = destinationPosition;

                m_destinationFinder.Cast();
                if (m_destinationFinder.isDetecting)
                {
                    m_hitBuffer = m_destinationFinder.GetValidHits();
                    var possibleDestination = m_hitBuffer[0].point;
                    Raycaster.SetLayerMask(LayerMask.GetMask("Environment"));
                    Raycaster.Cast(possibleDestination, Vector2.up, m_character.height, true, out int hitcount);
                    if (hitcount == 0)
                    {
                        m_climbToHere = possibleDestination;
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClimbSurface()
        {
            m_character.transform.position = m_climbToHere;
        }
    }
}
