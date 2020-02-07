using DChild.Gameplay.Characters;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class ReactivePlatform : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_particle;
        [SerializeField]
        private bool m_hasReactionAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, ShowIf("m_hasReactionAnimation")]
        private string m_reactionAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, ShowIf("m_hasReactionAnimation")]
        private string m_idleAnimation;

        private SpineRootAnimation m_animation;

        public event EventAction<EventActionArgs> OnReaction;

        private void Start()
        {
            m_animation = GetComponent<SpineRootAnimation>();
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            Debug.Log("afsfs");
            if (collider.enabled)
            {
                if (m_particle != null)
                {
                    Instantiate(m_particle, collider.collider.transform.position, Quaternion.identity);
                }
                if (m_hasReactionAnimation == true && m_reactionAnimation != string.Empty)
                {
                    m_animation.SetAnimation(0, m_reactionAnimation, false);
                    m_animation.AddAnimation(0, m_idleAnimation, true, 0.5f);
                }
                OnReaction?.Invoke(this, EventActionArgs.Empty);
            }
        }
    }
}