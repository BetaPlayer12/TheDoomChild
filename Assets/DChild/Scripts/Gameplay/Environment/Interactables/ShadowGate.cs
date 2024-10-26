﻿
/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Environment.Interractables;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class ShadowGate : MonoBehaviour, IButtonToInteract
    {
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
        private LocationPoster m_locationPoster;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public string promptMessage => "Enter";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        [Button, HideInEditorMode, HideIf("@!m_trigger.enabled")]
        public void CloseGate(bool instant)
        {
            StopAllCoroutines();
            if (instant)
            {
                var track = m_spineAnimation.AddAnimation(0, m_closeIdleAnimation, true, 0);
                track.MixDuration = 0;
                m_trigger.enabled = false;
            }
            else
            {
                m_trigger.enabled = false;
                StartCoroutine(TransistionAnimationRoutine(m_closeAnimation, m_closeIdleAnimation, false));
            }
        }

        [Button, HideInEditorMode, HideIf("@m_trigger.enabled")]
        public void OpenGate(bool instant)
        {
            StopAllCoroutines();
            if (instant)
            {
                var track = m_spineAnimation.AddAnimation(0, m_openIdleAnimation, true, 0);
                track.MixDuration = 0;
                m_trigger.enabled = true;
            }
            else
            {
                StartCoroutine(TransistionAnimationRoutine(m_openAnimation, m_openIdleAnimation, true));
            }
        }

        public void Interact(Character character)
        {
            GameplaySystem.gamplayUIHandle.OpenShadowGateMap(m_locationPoster.data.location);
        }

        private IEnumerator TransistionAnimationRoutine(string startAnimation, string idleAnimation, bool isTriggerEnabled)
        {
            m_spineAnimation.SetAnimation(0, startAnimation, false);
            m_spineAnimation.AddAnimation(0, idleAnimation, true, 0);
            yield return new WaitForAnimationComplete(m_spineAnimation.animationState, startAnimation);
            m_trigger.enabled = isTriggerEnabled;
        }

        private void OnArrival(object sender, CharacterEventArgs eventArgs)
        {
            //Ask Sr Martin what should happen when arriving at a shadow gate
        }

        private void Awake()
        {
            m_trigger = GetComponent<Collider2D>();
            m_spineAnimation = GetComponentInChildren<SpineAnimation>();
            CloseGate(true);
            m_locationPoster = GetComponent<LocationPoster>();
            m_locationPoster.data.OnArrival += OnArrival;
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}