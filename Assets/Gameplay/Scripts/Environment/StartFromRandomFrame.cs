using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFromRandomFrame : MonoBehaviour
{

    public Animator animator;

    void OnEnable()
    {
        if(animator)
        {
            //animator.Play("chain_swing", -1, Random.Range(0.0f, 1.0f));
        }
    }

}
