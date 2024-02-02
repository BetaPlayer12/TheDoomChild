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
    private Damageable m_playerdamageable;
    //Brute force temporarily
    [SerializeField]
    private ButodAI m_butodAI;
    [SerializeField, ReadOnly]
    private bool m_enemySighted;
    private bool m_playerAlreadyHit;
    private bool m_butodAlreadyMoved;
    [SerializeField, TabGroup("On Monster Sighted")]
    private UnityEvent m_onSeen;
    [SerializeField, TabGroup("On Monster Attack")]
    private UnityEvent m_onMonsterAttack;
    [SerializeField, TabGroup("On Player Hit")]
    private UnityEvent m_onPlayerHit;





    void Start()
    {
        m_playerdamageable = GameplaySystem.playerManager.player.character.GetComponent<Damageable>();
        m_butodAI = m_enemyGameObject.gameObject.GetComponentInParent<ButodAI>();
        m_enemySighted = false;
    }

    void LateUpdate()
    {

        if (m_enemyGameObject.isVisible)
        {
            if (m_enemySighted == false)
            {
                Debug.Log("Seen");
                m_enemySighted = true;
                m_onSeen?.Invoke();
            }
            if (m_butodAI == null)
            {

            }
            else
            {
                if(m_butodAlreadyMoved == false)
                {
                    if (m_butodAI.stateHandle == true)
                    {
                        m_onMonsterAttack?.Invoke();
                        m_butodAlreadyMoved = true;
                        Debug.Log("Miss u");
                    }
                }
                else
                {
                    if (m_playerAlreadyHit == false)
                    {
                        if (m_playerdamageable.health.currentValue < m_playerdamageable.health.maxValue)
                        {
                            m_onPlayerHit?.Invoke();
                            m_playerAlreadyHit = true;
                            Debug.Log("On hit");
                        }
                    }
                }
                
               



            }


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
