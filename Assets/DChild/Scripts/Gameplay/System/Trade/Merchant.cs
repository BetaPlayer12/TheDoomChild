
using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Trade
{
    public class Merchant : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private NPCProfile m_npcData;
        [SerializeField]
        private MerchantStore m_store;
        [SerializeField]
        private Vector3 m_promptOffset;
        [SerializeField]
        private bool m_hasDialogue;

        public event EventAction<EventActionArgs> InteractionOptionChange;

        public bool showPrompt => true;

        public string promptMessage => "Trade";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public void Interact(Character character)
        {
            if (m_hasDialogue)
            {
                GetComponent<NPCDialogue>().Interact(character);
            }
            else
            {
                CommenceTrade();
            }
        }

        [Button, HideInEditorMode]
        public void CommenceTrade()
        {
            GameplaySystem.gamplayUIHandle.OpenTradeWindow(m_npcData, m_store, m_store.tradeRates?.sellAskingPrice ?? null);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(promptPosition, 1f);
        }
    }
}
