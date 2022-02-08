using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using Holysoft;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.NavigationMap
{
    public class MapPointOfInterestTracker : MonoBehaviour
    {
    //public static TrackToggle trackToggleInstance;
        //public event EventAction<EventActionArgs> OnTrack;
        public static bool isTracked = false;

        public bool value => isTracked;

        [Button]
        public void Track()
        {
            isTracked = true;
            //OnTrack?.Invoke(this, EventActionArgs.Empty);
        }

        [Button]
        public void Untrack()
        {
            isTracked = false;
            //OnTrack?.Invoke(this, EventActionArgs.Empty);
        }
    }
}
