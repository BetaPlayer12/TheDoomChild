using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment.Interractables
{
    [RequireComponent(typeof(IHitToInteract))]
    public class HitInteractReaction : MonoBehaviour
    {
        [SerializeField]
        private bool m_isInverted;
        [SerializeField, TabGroup("Left")]
        private UnityEvent m_onHitFromLeft;
        [SerializeField, TabGroup("Right")]
        private UnityEvent m_onHitFromRight;

        private IHitToInteract m_interractable;

        private void Awake()
        {
            m_interractable = GetComponent<IHitToInteract>();
            m_interractable.OnHit += OnHit;
        }

        private void OnHit(object sender, HitDirectionEventArgs eventArgs)
        {
            if (eventArgs.direction == HorizontalDirection.Left)
            {
                if (m_isInverted)
                {
                    m_onHitFromRight?.Invoke();
                }
                else
                {
                    m_onHitFromLeft?.Invoke();
                }
            }
            else
            {
                if (m_isInverted)
                {
                    m_onHitFromLeft?.Invoke();
                }
                else
                {
                    m_onHitFromRight?.Invoke();
                }
            }
        }
    }
}