using DChild.Gameplay;
using DChild.Gameplay.Characters;
using UnityEngine;
using Refactor.DChild.Gameplay.Characters;

namespace Refactor.DChild.Gameplay
{
    public class CharacterSensors : MonoBehaviour
    {
        [SerializeField]
        private TurnHandle m_turn;

        private ISensorFaceRotation[] m_rotators;

        private void Awake()
        {
            m_rotators = GetComponentsInChildren<ISensorFaceRotation>();
            m_turn.TurnDone += OnCharacterTurn;
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            for (int i = 0; i < m_rotators.Length; i++)
            {
                m_rotators[i].AlignRotationToFacing(eventArgs.currentFacingDirection);
            }
        }
    }
}