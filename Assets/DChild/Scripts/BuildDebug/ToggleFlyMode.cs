using DChildDebug.Window;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFlyMode : MonoBehaviour, IToggleDebugBehaviour
{
    [SerializeField]
    public GameObject m_player;

    public bool value => throw new System.NotImplementedException();

    [Button]
    public void ToggleOn()
    {
       
        m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;

    }

    [Button]
    public void ToggleOff()
    {
       
        m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 20;
    }
}
