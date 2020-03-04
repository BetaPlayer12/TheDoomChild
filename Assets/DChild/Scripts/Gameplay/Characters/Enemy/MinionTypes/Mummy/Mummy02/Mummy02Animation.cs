using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Mummy02Animation : MummyAnimation
    {
        public override void DoIdle()
        {
            SetAnimation(0, "idle2", true);
        }
    }
}
