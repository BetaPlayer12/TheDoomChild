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
        private Transform m_prompt;

        public bool showPrompt => true;

        public string promptMessage => null;

        public Vector3 promptPosition => m_prompt.position;

        public void Interact(Character character)
        {
            m_entity.gameObject.SetActive(true);
        }
    }
}