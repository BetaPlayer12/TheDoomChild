using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class GoblinElevatorVisuals : MonoBehaviour
    {
            [SerializeField]
            private MovingPlatform m_elevator;
            [SerializeField]
            private SkeletonAnimation m_elevatoranimation;
            [SerializeField, HorizontalGroup("Up"), ShowIf("m_elevatoranimation"), Spine.Unity.SpineAnimation(dataField = "m_elevatoranimation.skeletonDataAsset")]
            private string m_upAnimation;
            [SerializeField, HorizontalGroup("Down"), ShowIf("m_elevatoranimation"), Spine.Unity.SpineAnimation(dataField = "m_elevatoranimation.skeletonDataAsset")]
            private string m_downAnimation;
            [SerializeField, HorizontalGroup("Idle"), ShowIf("m_elevatoranimation"), Spine.Unity.SpineAnimation(dataField = "m_elevatoranimation.skeletonDataAsset")]
            private string m_idleAnimation;

        [Button]
        public void moveUp()
        {
                m_elevatoranimation.state.SetAnimation(0, m_upAnimation, true);
        }
       [Button]
        public void moveDown()
        {
                m_elevatoranimation.state.SetAnimation(0, m_downAnimation, true);

        }
        [Button]
        public void pauseanimation()
        {
            m_elevatoranimation.state.SetAnimation(0, m_idleAnimation, true);
        }
 

        private void Start()
        {
            m_elevator.DestinationReached += destination;
            m_elevator.DestinationChanged += change;
        }

        private void change(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            Vector2 waypoint = m_elevator.GetWayPoint(eventArgs.currentWaypointIndex);
            float currentposition = this.transform.position.y;
            float elevatordestination = waypoint.y;
            if(elevatordestination > currentposition)
            {
                moveUp();
            }
            else
            {
                moveDown();
            }

        }

        private void destination(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            pauseanimation();
        }

        
    }
}
