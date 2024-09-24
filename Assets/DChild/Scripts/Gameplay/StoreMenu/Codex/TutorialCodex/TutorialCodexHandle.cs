using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Codex.Tutorial
{
    public class TutorialCodexHandle : CodexHandle<TutorialCodexData, TutorialCodexData>
    {
        public void SelectButton(TutorialCodexIndexButton button)
        {
            Select(button);
        }
    }

}
