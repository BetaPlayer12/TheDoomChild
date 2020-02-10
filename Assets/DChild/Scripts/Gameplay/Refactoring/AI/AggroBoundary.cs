using DChild.Gameplay;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.AI
{
    public abstract class AggroBoundary : MonoBehaviour
    {
        protected ICombatAIBrain m_brain;

        protected void Start()
        {
            m_brain = GetComponentInParent<ICombatAIBrain>();
        }

        protected void SetTargetToBrain(Collider2D spottedTarget, ITarget targetComponent)
        {
            if (targetComponent.CompareTag(Character.objectTag))
            {
                m_brain.SetTarget(spottedTarget.GetComponentInParent<IDamageable>(), spottedTarget.GetComponentInParent<Character>());
            }
            else
            {
                m_brain.SetTarget(spottedTarget.GetComponentInParent<IDamageable>());
            }
        }

        private void OnValidate()
        {
            gameObject.tag = "Sensor";
        }
    }
}