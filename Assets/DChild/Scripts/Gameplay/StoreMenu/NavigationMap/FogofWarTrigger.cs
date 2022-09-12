using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace DChild.Gameplay.NavigationMap
{
    public struct FogOfWarStateChangeEvent : IEventActionArgs
    {
        public FogOfWarStateChangeEvent(string varName, bool isRevealed)
        {
            this.varName = varName;
            this.isRevealed = isRevealed;
        }

        public string varName { get; }
        public bool isRevealed { get; }
    }

    public class FogofWarTrigger : MonoBehaviour
    {
        [SerializeField,VariablePopup(true), OnValueChanged("MatchGameObjectNameToVariable"),HideInPrefabAssets]
        private string m_varName;
        [ShowInInspector, HideInPrefabAssets]
        private bool m_isRevealed = false;
        private Collider2D m_trigger;

        public event EventAction<FogOfWarStateChangeEvent> RevealValueChange;

        public string varName => m_varName;
        public bool isRevealed => m_isRevealed;

        public void SetState(bool isRevealed)
        {
            SetStateAs(isRevealed);
            RevealValueChange?.Invoke(this, new FogOfWarStateChangeEvent(m_varName, m_isRevealed));
        }

        public void SetStateAs(bool isRevealed)
        {
            m_isRevealed = isRevealed;
            m_trigger.enabled = !isRevealed;
        }

        private void MatchGameObjectNameToVariable()
        {
            gameObject.name = NavMapUtility.GetObjectNameFromFogOfWarVariable(m_varName);
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
