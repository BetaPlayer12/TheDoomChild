using DChild.Gameplay;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPassableHandle : MonoBehaviour
{
    [SerializeField, InfoBox("These colliders will be ignored by the object"), TabGroup("EnemyColliders")]
    private Collider2D[] m_enemyColliders;
    [SerializeField]
    private Collider2D m_passableCollider;

    public void SetCollisions()
    {

        SetIgnoredCollisionState();

    }
    private void SetIgnoredCollisionState()
    {
        for (int j = 0; j < m_enemyColliders.Length; j++)
        {
            try
            {
                Physics2D.IgnoreCollision(m_passableCollider, m_enemyColliders[j], true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Enemy Environment Error Null Reference \n {e.Message}", this);
            }
        }

        GameplaySystem.playerManager.player.character.colliders.IgnoreCollider(m_passableCollider);
    }

    // Start is called before the first frame update
    private void Start()
    {

        SetCollisions();
    }

    private void OnDestroy()
    {
        GameplaySystem.playerManager.player.character.colliders.ClearIgnoredCollider(m_passableCollider);
    }

}
