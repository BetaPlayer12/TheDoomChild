using DChild.Gameplay;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterSightingHandle : MonoBehaviour
{

    [SerializeField]
    private bool m_oneTimeEncounter;
    [SerializeField]
    private Renderer m_enemyGameObject;
    [SerializeField]
    private ButodAI m_butodAI;
    [SerializeField, ReadOnly]
    private bool m_enemySighted;
    [SerializeField, TabGroup("On Monster Sighted")]
    private UnityEvent m_onSeen;
    [SerializeField, TabGroup("On Monster Attack")]
    private UnityEvent m_onMonsterAttack;

    //Brute force temporarily



    void Start()
    {
        m_butodAI = m_enemyGameObject.gameObject.GetComponentInParent<ButodAI>();
        m_enemySighted = false;
    }

    void LateUpdate()
    {

        if (m_enemyGameObject.isVisible)
        {
            if (m_enemySighted == false)
            {
                m_enemySighted = true;
                m_onSeen?.Invoke();
            }
            if(m_butodAI == null)
            {

            }
            else
            {
                if (m_butodAI.stateHandle == true)
                {
                    m_onMonsterAttack?.Invoke();
                }
            }
           

            Debug.Log("Seen");
        }
        else
        {
            if (m_oneTimeEncounter == false)
            {
                m_enemySighted = false;
            }
        }

    }
}
