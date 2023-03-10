using DChild.Gameplay.Characters.Players;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public struct FogOfWarStateChangeEvent : IEventActionArgs
    {
        public FogOfWarStateChangeEvent(Flag index, bool isRevealed)
        {
            this.index = index;
            this.isRevealed = isRevealed;
        }

        public Flag index { get; }
        public bool isRevealed { get; }
    }

    public class FogofWarTrigger : MonoBehaviour
    {
        [ShowInInspector, HideInPrefabAssets, OnValueChanged("UpdateState")]
        private bool m_isRevealed = false;
        private Collider2D m_trigger;

        private Flag m_flagIndex;

        public event EventAction<FogOfWarStateChangeEvent> RevealValueChange;
        public bool isRevealed => m_isRevealed;

        public void SetIndex(int index)
        {
            m_flagIndex = (Flag)(1 << index);
        }

        public void SetState(bool isRevealed)
        {
            SetStateAs(isRevealed);
            RevealValueChange?.Invoke(this, new FogOfWarStateChangeEvent(m_flagIndex, m_isRevealed));
        }

        public void SetStateAs(bool isRevealed)
        {
            m_isRevealed = isRevealed;
            m_trigger.enabled = !isRevealed;
        }

        public void SetStateAs(Flag state)
        {
            SetStateAs(state.HasFlag(m_flagIndex));
        }
        private void UpdateState()
        {
            SetState(m_isRevealed);
        }

        private void Awake()
        {
            m_trigger = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_isRevealed)
                return;

            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null && collision.tag != "Sensor")
            {
                SetState(true);
            }
        }

    }
}
