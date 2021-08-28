using Doozy.Engine;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public struct BrokenRealmGate : ISwitchHandle
    {
        [SerializeField]
        private Transform m_promptSource;
        [SerializeField]
        private Vector3 m_promptOffset;

        public bool isDebugSwitchHandle => true;

        public float transitionDelay => 0f;

        public bool needsButtonInteraction => true;

        public Vector3 promptPosition => m_promptSource.position + m_promptOffset;

        public string prompMessage => "Enter";

        public void DoSceneTransition(Character character, TransitionType type)
        {
            GameEventMessage.SendEvent("Show DebugMessage");
        }
    }
}
