using DChild.Gameplay.Characters;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment.Interractables
{
    [RequireComponent(typeof(IHitToInteract))]
    public class DropTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool m_isInverted;
        [SerializeField]
        private UnityEvent m_onLandDrop;

        private Vector3 validDirection = Vector3.up;
        private float contactThreshold = 30;


        private IHitToInteract m_interractable;
        private void Awake()
        {
            m_interractable = GetComponent<IHitToInteract>();
            m_interractable.OnHit += OnHit;
        }

        private void OnHit(object sender, HitDirectionEventArgs eventArgs)
        {
            m_onLandDrop.Invoke();
        }
    
   

        private void OnCollisionEnter2D(Collision2D collider)
        {

            //Check whether collider is coming from opposite side of effector
            if (LayerMask.LayerToName(collider.gameObject.layer) == "Player")
            {
               
                        m_onLandDrop.Invoke();
                
            }
        }


    }
}
