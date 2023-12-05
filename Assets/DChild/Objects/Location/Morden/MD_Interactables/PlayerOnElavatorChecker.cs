using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnElavatorChecker : MonoBehaviour
{
    public bool isonwhentransferred = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.tag != "Sensor")
        {
            DialogueLua.SetVariable("IsonMordenElevator", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Sensor"&& isonwhentransferred == false)
        {
            DialogueLua.SetVariable("IsonMordenElevator", false);
        }
    }
    public void isonpassageway()
    {
        isonwhentransferred = true;
    }
    public void isnotonpassageway()
    {
        isonwhentransferred = false;
    }
}
