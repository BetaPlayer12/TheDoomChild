using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Mummy01Animation : MummyAnimation
    {
        public const string ANIMATION_STAB_ATTACK = "attack";

        public void DoStab()
        {
            SetAnimation(0, ANIMATION_STAB_ATTACK, false);
        }

        public override void DoIdle()
        {
            SetAnimation(0, "idle1", true);
        }
    }
}
