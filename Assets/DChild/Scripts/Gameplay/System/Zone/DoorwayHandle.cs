using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class DoorwayHandle : ISwitchHandle
    {
        [SerializeField]
        private Transform m_prompt;
        [SerializeField]
        private bool m_forceExitFacing;
        [SerializeField, ShowIf("m_forceExitFacing")]
        private HorizontalDirection m_exitFacing;

        public float transitionDelay => 0;

        public bool needsButtonInteraction => true;

        public Vector3 promptPosition => m_prompt.position;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            if (type == TransitionType.Exit && m_forceExitFacing)
            {
                character.SetFacing(m_exitFacing);
            }
        }
    }
}
