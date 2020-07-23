﻿using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using Doozy.Engine.UI;

public class RoomText : MonoBehaviour, ISerializableComponent
{
    public struct SaveData : ISaveData
    {
        [SerializeField]
        private bool m_Shown;
        
        

        public SaveData(bool m_Shown)
        {
            this.m_Shown = m_Shown;
        }

        public bool Shown => m_Shown;
    }

    [SerializeField]
    private bool m_Shown;
    [SerializeField]
    private UIView m_UI;
    [SerializeField, MinValue(0)]
    private float m_TextFadeDelay;

    public ISaveData Save()
    {
        return new SaveData(m_Shown);
    }

    public void Load(ISaveData data)
    {
        var saveData = (SaveData)data;
        m_Shown = saveData.Shown;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
   if (collision.isTrigger)
        {
            if (m_Shown != true)
            {
                //show animation
                m_UI.Show();
                m_Shown = true;
                StartCoroutine(DelayedFade());

            }          
        }
    }

    private IEnumerator DelayedFade()
    {
        yield return new WaitForSeconds(m_TextFadeDelay);
        m_UI.Hide();
    }
}
