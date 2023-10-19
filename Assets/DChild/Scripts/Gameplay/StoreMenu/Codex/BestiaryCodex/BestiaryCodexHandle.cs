using DChild.Menu.Bestiary;

namespace DChild.Menu.Codex.Bestiary
{
    public class BestiaryCodexHandle : CodexHandle<BestiaryData, BestiaryData> 
    {
        public void SelectButton(BestiaryCodexIndexButton button)
        
        {
            Select(button);
        }
    }
}