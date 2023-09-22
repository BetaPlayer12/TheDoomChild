using DChild.Codex.Characters;
using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Codex.Characters
{
    public class CharacterCodexHandle : CodexHandle<CharacterCodexData, CharacterCodexData>
    {
        public void Select(CharacterCodexIndexButton button)
        {
            Select(button);
        }

    }

}
