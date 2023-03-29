using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanimEventToSetAnimParam : MonoBehaviour
{
    public Animator targetAnimator; // The animator component to set the int value on
    public string targetParamName; // The name of the int parameter to set on the target animator
    public int setValue; // The value to set the parameter to when the event is detected
    public float resetTime = 1.0f; // The time to wait before resetting the parameter value

    private int originalValue; // The original value of the parameter before it was set

    // Start is called before the first frame update
    void Start()
    {
        // Store the original value of the parameter
        originalValue = targetAnimator.GetInteger(targetParamName);
    }

    // This function is called by the animator when the event is detected
    public void OnMecanimEvent()
    {
        // Set the parameter value on the target animator
        targetAnimator.SetInteger(targetParamName, setValue);

        // Start the coroutine to reset the value after the specified time
        StartCoroutine(ResetParamValue());
    }

    IEnumerator ResetParamValue()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(resetTime);

        // Reset the parameter value on the target animator to its original value
        targetAnimator.SetInteger(targetParamName, originalValue);
    }
}
