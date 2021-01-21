using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShake : MonoBehaviour
{
    
    Vector2 startingPos;

    private IHitToInteract m_interractable;
  
   
    private void Awake()
    {
        m_interractable = GetComponent<IHitToInteract>();
        m_interractable.OnHit += OnHit;
        startingPos.x = transform.position.x;
        startingPos.y = transform.position.y;
    }

    private void OnHit(object sender, HitDirectionEventArgs eventArgs)
    {
        StartCoroutine(Shake());
    }
    IEnumerator Shake()
    {
        yield return new WaitForSeconds(0);
        float timePassed = 0;
        while (timePassed < 1)
        {
            timePassed += Time.deltaTime;
            transform.position = new Vector2(startingPos.x, startingPos.y) + Random.insideUnitCircle * 1;
            yield return null;
        }
    }
}
