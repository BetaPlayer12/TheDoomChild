using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowAnimationEvent : MonoBehaviour
{
    private Thief _thief;

    private void Start()
    {
        _thief = transform.parent.GetComponent<Thief>();
    }
    public void Fire()
    {
     
        _thief.Attack();
    }
}
