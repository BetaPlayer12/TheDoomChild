using PixelCrushers.DialogueSystem;

namespace DChild.UI
{
    public class DChildStandardUIContinueButtonFastForward : StandardUIContinueButtonFastForward
    {
       public static AbstractTypewriterEffect currentTypewriterEffect { get; private set; }

        public override void Awake()
        {
            base.Awake();
            currentTypewriterEffect = typewriterEffect;
        }
    }
}