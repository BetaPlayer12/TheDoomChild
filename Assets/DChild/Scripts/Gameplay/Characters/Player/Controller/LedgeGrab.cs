using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LedgeGrab : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        private IsolatedPhysics2D m_characterPhysics;
        private IFacing m_characterFacing;

        private RaySensor m_ledgeSensorCliff;
        private RaySensor m_ledgeSensorEdge;
        private ILedgeGrabState m_state;

        public void ConnectEvents()
        {
            var ledgeGrabController = GetComponentInParent<IledgeController>();
            ledgeGrabController.LedgeGrabCall += PullFromCliff;
            Debug.Log("Event check");
        }

        public void Initialize(IPlayerModules player)
        {
            m_characterFacing = player;
            m_characterPhysics = player.physics;
            m_ledgeSensorCliff = player.sensors.ledgeSensorCliff;
            m_ledgeSensorEdge = player.sensors.ledgeSensorEdge;
            m_state = player.characterState;
        }

        public void PullFromCliff()
        {
            m_ledgeSensorCliff.Cast();
            m_ledgeSensorEdge.Cast();
            Debug.Log("Check ledge");

            if (m_ledgeSensorCliff.isDetecting == true && m_ledgeSensorEdge.isDetecting == false)
            {

                var snapTo = m_characterPhysics.position;
                snapTo.x += m_characterFacing.currentFacingDirection == HorizontalDirection.Right ? 3 : -3;
                snapTo.y += 3;
                m_characterPhysics.position = snapTo;
                m_state.isLedging = true;
                StartCoroutine(ChangeLedgingStateToFalse());
            };
        }

        private void PullFromCliff(object sender, EventActionArgs eventArgs)
        {
            PullFromCliff();
        }

        private IEnumerator ChangeLedgingStateToFalse()
        {
            yield return new WaitForEndOfFrame();
            m_state.isLedging = false;
        }
    }
}