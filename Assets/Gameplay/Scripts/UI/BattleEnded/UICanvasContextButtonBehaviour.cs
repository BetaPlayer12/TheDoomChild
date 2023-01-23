using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Inspector;

public class UICanvasContextButtonBehaviour : MonoBehaviour
{

    public UnityEvent canvasContextButtonEvent;

    public void ContextButtonPressed()
    {
        canvasContextButtonEvent.Invoke();
    }



}
