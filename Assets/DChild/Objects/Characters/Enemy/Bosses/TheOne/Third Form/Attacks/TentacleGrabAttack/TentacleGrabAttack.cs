using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleGrabAttack : MonoBehaviour, IEyeBossAttacks
{
    [SerializeField]
    private TentacleGrab m_tentacleGrab;

    public IEnumerator ExecuteAttack()
    {
        //Make it so different types of grab attacks can happen later
        m_tentacleGrab.GroundSlamAttack();
        yield return null;
    }

    public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
    {
        throw new System.NotImplementedException();
    }
}
