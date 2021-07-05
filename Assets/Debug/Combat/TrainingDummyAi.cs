using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment.Interractables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyAi : MonoBehaviour
{
    [SerializeField]
    private Damageable m_dummy;
    private bool m_hit = false;
    [SerializeField]
    public float m_HealDelay;

    

    private void Awake()
    {
        m_dummy = GetComponent<Damageable>();
        m_dummy.DamageTaken += OnHits;
    }

    private void OnHits(object sender, Damageable.DamageEventArgs eventArgs)
    {
        m_hit = true;
        Debug.Log("it worked he got hit");
    }

   
    IEnumerator DelayCoroutine()
    {

        yield return new WaitForSeconds(m_HealDelay);
        m_dummy.Heal(9999);
        m_hit = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_hit == true)
        {
            StartCoroutine(DelayCoroutine());
        }
    }
}
