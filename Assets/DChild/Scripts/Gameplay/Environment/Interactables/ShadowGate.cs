/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Environment.Interractables;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class ShadowGate : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private Location m_fromLocation;
        [SerializeField]
        private Vector3 m_promptOffset;

        [SerializeField, Spine.Unity.SpineAnimation, BoxGroup("Animation")]
        private string m_closeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, BoxGroup("Animation")]
        private string m_closeIdleAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, BoxGroup("Animation")]
        private string m_openAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, BoxGroup("Animation")]
        private string m_openIdleAnimation;

        private Collider2D m_trigger;
        private SpineAnimation m_spineAnimation;

        public bool showPrompt => true;

        public string promptMessage => "Enter";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public void CloseGate(bool instant)
        {

        }

        public void OpenGate(bool instant)
        {

        }


        public void Interact(Character character)
        {
            GameplaySystem.gamplayUIHandle.OpenWorldMap(m_fromLocation);
        }

        private IEnumerator TransistionAnimationRoutine(string startAnimation, string idleAnimation, bool isTriggerEnabled)
        {
            m_spineAnimation.SetAnimation(0,startAnimation,false)
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}