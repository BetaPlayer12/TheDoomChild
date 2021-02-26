using DChild.Gameplay.Environment.Interractables;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.NPC
{
    public class NPCDialogue : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private bool m_hasDialogue = true;
        [SerializeField,ShowIf("m_hasDialogue")]
        private bool m_hasConditionForDialogue;
        [SerializeField]
        private Vector3 m_promptOffset;

        private DialogueSystemTrigger m_trigger;

        public bool hasDialogue
        {
            set => m_hasDialogue = value;
        }

        public bool showPrompt
        {
            get
            {
                if (m_hasDialogue)
                {
                    if (m_hasConditionForDialogue)
                    {
                        return m_trigger.condition.IsTrue(GameplaySystem.playerManager.player.character.transform);
                    }
                    return true;
                }
                return false;
            }
        }

        public string promptMessage => "Talk";

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public void Interact(Character character)
        {
            m_trigger.OnUse(character.transform);
        }

        private void Awake()
        {
            m_trigger = GetComponent<DialogueSystemTrigger>();
            m_trigger.conversationActor = GameplaySystem.playerManager.player.character.transform;
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.DrawIcon(position + Vector3.up * 1.5f, "DialogueDatabase Icon.png",false);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}