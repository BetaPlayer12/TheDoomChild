using DChild.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField]
    private Transform m_playerObject;
    [SerializeField]
    private float m_offsetX;
    [SerializeField]
    private float m_offsetY;
    [SerializeField]
    private List<Vector2> m_playerPositions;

    private void FollowObject()
    {
        if (m_playerObject.lossyScale.x < 0)
        {
            gameObject.transform.position = new Vector2(m_playerObject.position.x + m_offsetX, m_playerObject.position.y + m_offsetY);
        }
        else
        {
            gameObject.transform.position = new Vector2(m_playerObject.position.x - m_offsetX, m_playerObject.position.y + m_offsetY);

        }

    }
    void Start()
    {
        var playerObject = GameplaySystem.playerManager.player.character.centerMass;
        m_playerObject = playerObject;
    }

    void Update()
    {
        FollowObject();
    }
}
