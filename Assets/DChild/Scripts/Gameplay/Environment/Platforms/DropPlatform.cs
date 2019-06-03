using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Platforms
{
    [RequireComponent(typeof(Collider2D), typeof(PlatformEffector2D))]
    public class DropPlatform : MonoBehaviour
    {

        private void OnValidate()
        {
            gameObject.tag = "Droppable";
            GetComponent<Collider2D>().usedByEffector = true;
            GetComponent<PlatformEffector2D>().useOneWay = true;
        }
    }

}