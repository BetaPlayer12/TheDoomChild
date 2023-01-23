using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextBehaviour : MonoBehaviour
{

    [Header("Component Reference")]
    public TextMeshProUGUI textDisplay;

    public void SetText(string newText)
    {
        textDisplay.SetText(newText);
    }
    
}
