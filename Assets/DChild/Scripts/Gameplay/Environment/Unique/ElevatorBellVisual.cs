using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class ElevatorBellVisual : MonoBehaviour
    {
        
        [SerializeField]
        private SkeletonAnimation m_elevatoranimation;
        [SerializeField, HorizontalGroup("Up"), ShowIf("m_elevatoranimation"), Spine.Unity.SpineAnimation(dataField = "m_elevatoranimation.skeletonDataAsset")]
        private string m_flinchAnimation;
        [SerializeField, HorizontalGroup("Down"), ShowIf("m_elevatoranimation"), Spine.Unity.SpineAnimation(dataField = "m_elevatoranimation.skeletonDataAsset")]
        private string m_idleAnimation;



        [Button]
        public void flinch()
        {
            m_elevatoranimation.state.SetAnimation(1, m_flinchAnimation, false);
            StartCoroutine(waiting());
        }
        [Button]
        public void idle()
        {
            m_elevatoranimation.state.SetAnimation(1, m_idleAnimation, true);

        }
        IEnumerator waiting()
        {
            yield return new WaitForSeconds(1);
            idle();
        }
        private void Start()
        {
            idle();
        }
    }
}
