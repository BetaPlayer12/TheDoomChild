using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Items
{
    public  class ShadowPetHandler : MonoBehaviour
    {
        public event EventAction<EventActionArgs> Desummoning;

        public virtual void PetDesummon()
        {
            Desummoning?.Invoke(this, EventActionArgs.Empty);
        }
    }
}
