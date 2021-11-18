using DChild.Gameplay;
using DChild.Gameplay.Characters.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlatform : MonoBehaviour
{
    private void OnValidate()
    {
        if (tag != "Droppable")
            tag = "Droppable";

        if (Application.isPlaying)
        {
            Destroy(this);
        }
    }
}
