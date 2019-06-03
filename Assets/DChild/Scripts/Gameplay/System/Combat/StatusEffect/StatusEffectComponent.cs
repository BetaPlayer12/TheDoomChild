using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public abstract class StatusEffectComponent : MonoBehaviour
    {
        protected IStatusEffectEvents m_effect { get; private set; }

        protected abstract void OnEffectEnd(object sender, StatusEffectEventArgs eventArgs);
        protected abstract void OnEffectStart(object sender, StatusEffectEventArgs eventArgs);

        private void Awake()
        {
            m_effect = GetComponentInParent<IStatusEffectEvents>();
        }

        private void OnEnable()
        {
            m_effect.EffectStart += OnEffectStart;
            m_effect.EffectEnd += OnEffectEnd;
        }


        private void OnDisable()
        {
            m_effect.EffectStart -= OnEffectStart;
            m_effect.EffectEnd -= OnEffectEnd;
        }
    }
}