using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlatform : MonoBehaviour
{
    private Collider2D m_collider;
    public float waitTime = 0.5f;

    private Collider2D m_playerCollider;
    private bool m_inContact;
    private float m_timer;

    // Start is called before the first frame update
    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_inContact)
        {
            float verticalInput = Input.GetAxis("Vertical");

            if (verticalInput < 0)
            {
                if (Input.GetButton("Jump"))
                {
                    
                    Physics2D.IgnoreCollision(m_playerCollider, m_collider, true);
                    m_inContact = false;
                    m_timer = waitTime;
                }

            }
        }

        if (m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0)
            {
                Physics2D.IgnoreCollision(m_playerCollider, m_collider, false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_inContact == false)
        {
            if (collision.enabled)
            {
                if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
                {
                    m_playerCollider = collision.collider;
                    m_inContact = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (m_inContact)
        {
            if (m_playerCollider == collision.collider)
            {
                m_inContact = false;
                Physics2D.IgnoreCollision(m_playerCollider, m_collider, false);
            }
        }
    }

    private void OnValidate()
    {
        tag = "Droppable";
    }
}
