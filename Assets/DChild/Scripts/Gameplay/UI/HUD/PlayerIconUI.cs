using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DChild.Gameplay.UI
{
    public class PlayerIconUI : StylizedPlayerUI<string>
    {
        [SerializeField,PropertyOrder(0)]
        private SkeletonGraphic m_graphic;

        [SerializeField, Spine.Unity.SpineAnimation, ListDrawerSettings(OnBeginListElementGUI = "OnAnimationBeginElement", HideAddButton = true, HideRemoveButton = true), PropertyOrder(2)]
        protected string[] m_list;

        protected override string[] list { get => m_list; set => m_list = value; }

        public override void ChangeTo(PlayerUIStyle type)
        {
            m_graphic.AnimationState.SetAnimation(0, m_list[(int)type], true);
        }
    }
}