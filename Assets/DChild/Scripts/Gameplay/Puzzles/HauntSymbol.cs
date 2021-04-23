using System;
using DChild.Gameplay.Environment.Interractables;
using UnityEngine;

namespace DChild.Gameplay.Puzzles
{
    public class HauntSymbol : MonoBehaviour, IButtonToInteract
    {
        [SerializeField]
        private GameObject m_entity;
        [SerializeField]
        private Vector3 m_promptOffset;

        public bool showPrompt => true;

        public string promptMessage => null;

        public Vector3 promptPosition => transform.position + m_promptOffset;

        public void Interact(Character character)
        {
            m_entity.gameObject.SetActive(true);
        }

        private void OnDrawGizmosSelected()
        {
            var position = promptPosition;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 1f);
        }
    }
}