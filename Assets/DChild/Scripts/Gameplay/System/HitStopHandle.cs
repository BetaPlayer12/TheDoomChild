using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class HitStopHandle : MonoBehaviour
    {
        [SerializeField]
        private CountdownTimer m_duration;

        [Button]
        public void Execute()
        {
            GameTime.RegisterValueChange(this, 0, GameTime.Factor.Multiplication);
            m_duration.Reset();
            enabled = true;
        }



        //private void Awake()
        //{
        //    m_duration.CountdownEnd += ResumeTime;
        //    enabled = false;
        //}

        //private void Update()
        //{
        //    if (GameplaySystem.isGamePaused == false)
        //    {
        //        m_duration.Tick(Time.unscaledDeltaTime);
        //    }
        //}

        //private void OnDestroy()
        //{
        //    GameTime.UnregisterValueChange(this, GameTime.Factor.Multiplication);
        //}
    }

}