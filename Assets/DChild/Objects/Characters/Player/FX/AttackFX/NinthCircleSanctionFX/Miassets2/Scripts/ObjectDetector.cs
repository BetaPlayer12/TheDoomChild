using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public GameObject objectToDetect;
    public Animator targetAnimator;
    public float delayTime = 1.0f;
    public string intParameterName = "SomeIntParameter";

    private bool objectIsActive = false;
    private float timeSinceActivation = 0.0f;

    void Update()
    {
        // Check if objectToDetect is active
        if (objectToDetect.activeInHierarchy != objectIsActive)
        {
            objectIsActive = objectToDetect.activeInHierarchy;

            // If object is activated, set animator parameter to 1
            if (objectIsActive)
            {
                targetAnimator.SetInteger(intParameterName, 1);
                timeSinceActivation = 0.0f;
            }
        }

        // If object was recently activated, wait for delayTime before setting animator parameter to 2
        if (objectIsActive && timeSinceActivation < delayTime)
        {
            timeSinceActivation += Time.deltaTime;

            if (timeSinceActivation >= delayTime)
            {
                targetAnimator.SetInteger(intParameterName, 2);
            }
        }
    }
}
