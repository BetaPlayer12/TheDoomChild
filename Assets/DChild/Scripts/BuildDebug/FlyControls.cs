using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyControls : MonoBehaviour
{

    [SerializeField]
    public GameObject m_player;
  
    void Update()
    {
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 position = m_player.transform.position;
                position.x--;
                m_player.transform.position = position;
            }

            if (Input.GetKey(KeyCode.D))
            {
                Vector3 position = m_player.transform.position;
                position.x++;
                m_player.transform.position = position;
            }

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
        }
}
