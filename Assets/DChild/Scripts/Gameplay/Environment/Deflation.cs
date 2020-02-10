using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deflation : MonoBehaviour
{
    [SerializeField, TabGroup("SolidState")]
    private UnityEvent SolidState;
    [SerializeField, TabGroup("FadingState")]
    private UnityEvent FadingState;
    [SerializeField, TabGroup("Transparent state")]
    private UnityEvent TransparentState;

    public SpriteRenderer SR;
    public bool transparentCheck;
    Color CR;
    public bool fadeTransition;
    public  bool TransparentCon = false;


    private void Start()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
       
            FadingState?.Invoke();
            TransparentCon = false;
            transparentCheck = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        SolidState?.Invoke();
        transparentCheck = false;

    }

    private void Update()
    {
       
            
                if (transparentCheck)
                {
                    TransparentState?.Invoke();
                    TransparentCon = true;
                    fadeTransition = false;
                }
           
           
    }


}
