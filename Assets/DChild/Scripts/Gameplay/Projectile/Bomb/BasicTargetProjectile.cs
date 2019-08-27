using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTargetProjectile : MonoBehaviour
{
    [SerializeField]
    private MechanicalRobotAI m_playerInfo;



    private Vector2 m_playerLocation;
    private float m_projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_playerLocation = m_playerInfo.GetPlayerTransform();
        m_projectileSpeed = m_playerInfo.GetProjectileSpeed();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, m_playerLocation, m_projectileSpeed);

    }
}
