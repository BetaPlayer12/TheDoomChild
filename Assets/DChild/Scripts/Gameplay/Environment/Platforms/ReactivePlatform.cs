using DChild.Gameplay.Characters;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class ReactivePlatform : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_particle;
        [SerializeField]
        private bool m_reactToCharactersOnly;
        [SerializeField]
        private bool m_hasReactionAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, ShowIf("m_hasReactionAnimation")]
        private string m_reactionAnimation;
        [SerializeField, Spine.Unity.SpineAnimation, ShowIf("m_hasReactionAnimation")]
        private string m_idleAnimation;
        [SerializeField]
        private UnityEvent m_otherReaction;

        private SkeletonAnimation m_animation;

        public event EventAction<CollisionEventActionArgs> OnReaction;

        private void Start()
        {
            m_animation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.enabled)
            {
                if (m_reactToCharactersOnly)
                {
                    if (collider.collider.TryGetComponentInParent(out Character character) == false)
                    {
                        return;
                    }
                }

                if (m_particle != null)
                {
                    Instantiate(m_particle, collider.collider.transform.position, Quaternion.identity);
                }
                if (m_hasReactionAnimation == true && m_reactionAnimation != string.Empty)
                {
                    m_animation.state.SetAnimation(0, m_reactionAnimation, false);
                    m_animation.state.AddAnimation(0, m_idleAnimation, true, 0);
                }
                m_otherReaction?.Invoke();
                using (Cache<CollisionEventActionArgs> cacheEvent = Cache<CollisionEventActionArgs>.Claim())
                {
                    cacheEvent.Value.Set(collider);
                    OnReaction?.Invoke(this, cacheEvent.Value);
                    Cache<CollisionEventActionArgs>.Release(cacheEvent);
                }
            }
        }
    }
}