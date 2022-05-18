using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class GoblinElevatorVisuals : MonoBehaviour
    {
        [System.Serializable]
        public class DoorPanel
        {
            [SerializeField]
            private SkeletonAnimation m_elevator;
            [SerializeField, HorizontalGroup("Up"), ShowIf("m_elevator"), Spine.Unity.SpineAnimation(dataField = "m_elevator.skeletonDataAsset")]
            private string m_upAnimation;
            [SerializeField, HorizontalGroup("Down"), ShowIf("m_elevator"), Spine.Unity.SpineAnimation(dataField = "m_elevator.skeletonDataAsset")]
            private string m_downAnimation;

            public void SetAs(bool isGoingUp)
            {
                var chosenAnimation = isGoingUp ? m_upAnimation : m_downAnimation;
                m_elevator.state.SetAnimation(0, chosenAnimation, false);
            }
        }
    }
}
