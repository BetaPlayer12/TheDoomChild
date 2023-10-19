using DChild.Menu.Codex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Codex.LocationCodex
{
    public class LocationCodexHandle : CodexHandle<LocationCodexData, LocationCodexData>
    {
        public void SelectButton(LocationCodexIndexButton button)
        {
            Select(button);
        }

    }
}

