using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Mummy02 : Mummy
    {
        private Mummy02Animation m_anim;

        protected override MummyAnimation m_animation => m_anim;

        protected override void Awake()
        {
            base.Awake();
            m_anim = GetComponent<Mummy02Animation>();
        }
    }
}
