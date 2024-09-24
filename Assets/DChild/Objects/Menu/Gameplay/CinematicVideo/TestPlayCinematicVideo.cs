using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Gameplay.Systems
{
    public class TestPlayCinematicVideo : MonoBehaviour
    {
        [SerializeField]
        private VideoClip m_videoPlayer;
 

        private void Start()
        {
            
        }

        [Button]
        public void PlayCinematicVideo()
        {
            GameplaySystem.gamplayUIHandle.ShowCinematicVideo(m_videoPlayer, null, null);
        }


    }
}

