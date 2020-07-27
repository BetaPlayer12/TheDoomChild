using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInterface : MonoBehaviour
{
    [SerializeField]
    private KeyCode[] buttons;
    [SerializeField]
    private UIView[] ScreenItem;


    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey)
        {
            buttoncheck(e.keyCode);
            Debug.Log("Detected key code: " + e.keyCode);
        }
    }
    private void buttoncheck(KeyCode check)
    {
        for (int i = 0; i >= buttons.Length; i++)
        {
            if (check == buttons[i])
            {
                Debug.Log("Detected key code: " + buttons[i]);
            }
        }
    }
}
