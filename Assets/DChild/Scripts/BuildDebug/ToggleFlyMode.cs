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
    public bool m_fly=false;

    public bool value => m_player.GetComponentInChildren<Rigidbody2D>().gravityScale == 0;

    [Button]
    public void ToggleOn()
    {
       
        m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
        m_fly = true;

    }

    [Button]
    public void ToggleOff()
    {
       
        m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 20;
        m_fly = false;
    }
    void Update()
    {
        if (m_fly == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 position = m_player.transform.position;
                position.y++;
                m_player.transform.position = position;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Vector3 position = m_player.transform.position;
                position.y--;
                m_player.transform.position = position;
            }
            if (Input.GetKey(KeyCode.D))
            {
                Vector3 position = m_player.transform.position;
                position.x++;
                m_player.transform.position = position;
            }
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 position = m_player.transform.position;
                position.x--;
                m_player.transform.position = position;
            }
        }

    }
 }
