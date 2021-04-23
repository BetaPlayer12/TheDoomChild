using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDeathHitDetector : MonoBehaviour
{
    public event EventAction<EventActionArgs> PlayerHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHit?.Invoke(this, new EventActionArgs());
    }
}
