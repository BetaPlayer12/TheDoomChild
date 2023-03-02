using Doozy.Runtime.Signals;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace DChild.UI
{
    public class DChildStandardUIContinueButtonFastForward : StandardUIContinueButtonFastForward
    {
        public static AbstractTypewriterEffect currentTypewriterEffect { get; private set; }

        [SerializeField]
        private SignalSender m_continueDialogueSignal;

        protected override void ContinueConversation()
        {
            base.ContinueConversation();
            m_continueDialogueSignal.SendSignal();
        }

        public override void Awake()
        {
            base.Awake();
            currentTypewriterEffect = typewriterEffect;
        }

    }
}