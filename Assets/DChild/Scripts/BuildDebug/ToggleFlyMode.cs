using DChildDebug.Window;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFlyMode : MonoBehaviour, IToggleDebugBehaviour
{
    [SerializeField]
    public GameObject m_player;
    [SerializeField]
    private FlyControls m_controller;
    public bool value => m_player.GetComponentInChildren<Rigidbody2D>().gravityScale == 0;

    [Button]
    public void ToggleOn()
    {
       
        m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
        m_controller.enabled = true;
    }

    [Button]
    public void ToggleOff()
    {
       
        m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 20;
        m_controller.enabled = false;
    }
   
 }
