using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageBehaviour : MonoBehaviour
{

    public Image imageDisplay;

    public void SetImage(Sprite newImage)
    {
        imageDisplay.sprite = newImage;
    }
    
}
