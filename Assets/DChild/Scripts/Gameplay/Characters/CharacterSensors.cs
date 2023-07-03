using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class CharacterSensors : MonoBehaviour
    {
        private ISensorFaceRotation[] m_rotators;

        private void Awake()
        {
            m_rotators = GetComponentsInChildren<ISensorFaceRotation>();
            var turningCharacter = GetComponentInParent<ITurningCharacter>();
            if (turningCharacter != null)
            {
                turningCharacter.CharacterTurn += OnCharacterTurn;
            }
            else
            {
                Debug.LogError("Null Reference on Turning Character", this);
            }
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
