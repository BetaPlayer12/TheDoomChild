using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSlashImpactFX : MonoBehaviour
{
    [SerializeField]
    private Animator m_fxAnimator;
    [SerializeField]
    private int m_maxCounts;

    private void Awake()
    {
        int lineDirection = UnityEngine.Random.Range(0, m_maxCounts);

        m_fxAnimator.SetInteger("FinalSlashLineDirection", lineDirection);
    }
}
