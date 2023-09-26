using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Codex.Lore
{
    public class LoreCodexHandle : CodexHandle<LoreCodexData, LoreCodexData>
    {
        public void SelectButton(LoreCodexIndexButton button)
        {
            Select(button);
        }


    }
}

