using DChild.Gameplay;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatformPassagewayHandler : MonoBehaviour
{
    
    [SerializeField, ListDrawerSettings(CustomAddFunction = "AddWaypoint", OnBeginListElementGUI = "OnBeginWayPointElementGUI", OnEndListElementGUI = "OnEndWayPointElementGUI"), TabGroup("Setting"), HideInInlineEditors]
    private Vector2 m_startwaypoint;
    [SerializeField]
    private UnityEvent m_onentry;
    // Start is called before the first frame update
    void Start()
    {
       bool wasonelevator= DialogueLua.GetVariable("IsonMordenElevator").AsBool;
        if (wasonelevator == true)
        {
            GameplaySystem.playerManager.player.character.transform.position  = m_startwaypoint;
            m_onentry?.Invoke();
        }
    }

   
}
