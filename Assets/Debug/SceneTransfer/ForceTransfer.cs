﻿using DChild.Gameplay;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class SceneTransferDebugger : MonoBehaviour
    {
        [SerializeField]
        private LocationSwitcher m_switcher;
        [SerializeField]
        private Character m_character;

        [Button]
        public void Switch()
        {
            m_switcher.GoToDestination(m_character);
        }
    }

}