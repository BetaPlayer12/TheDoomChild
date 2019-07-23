using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class CharacterSensors : MonoBehaviour, IFacingComponent
    {
        private ISensorFaceRotation[] m_rotators;

        public void Update(HorizontalDirection facing)
        {
            if (m_rotators == null)
            {
                m_rotators = GetComponentsInChildren<ISensorFaceRotation>();
            }

            for (int i = 0; i < m_rotators.Length; i++)
            {
                m_rotators[i].AlignRotationToFacing(facing);
            }
        }

        private void Awake()
        {
            m_rotators = GetComponentsInChildren<ISensorFaceRotation>();
            GetComponentInParent<ITurningCharacter>().CharacterTurn += OnCharacterTurn;
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
