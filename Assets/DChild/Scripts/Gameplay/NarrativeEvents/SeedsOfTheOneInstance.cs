using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsOfTheOneInstance : MonoBehaviour
{
    [SerializeField]
    private bool m_isDead;

    [Button]
    public void SetSeedAsDead()
    {
        m_isDead = true;
    }

    public bool IsSeedDead()
    {
        if (m_isDead)
            return true;
        else
            return false;
    }
}
