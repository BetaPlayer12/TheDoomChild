using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Holysoft;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.NavigationMap
{
    public class FogofWarTrigger : MonoBehaviour
    {   
        //public static RevealToggle revealToggleInstance;
        //public event EventAction<EventActionArgs> OnReveal;
        public static bool revealValue = false;

        public bool value => revealValue;

        [Button]
        public void Reveal()
        {
            revealValue = true;
            //OnReveal?.Invoke(this, EventActionArgs.Empty);
        }

        [Button]
        public void Hide()
        {
            revealValue = false;
            //OnReveal?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            var playerObject = collision.gameObject.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null && collision.tag != "Sensor")
            {
                Reveal();
            }
        }
       
    }
}
