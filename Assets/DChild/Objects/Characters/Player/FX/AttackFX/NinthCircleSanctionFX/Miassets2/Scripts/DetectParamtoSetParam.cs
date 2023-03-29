using UnityEngine;

public class DetectParamtoSetParam : MonoBehaviour
{
    [SerializeField] private GameObject objectToCheck;
    [SerializeField] private string parameterNameToCheck;
    [SerializeField] private int valueToCheck;
    [SerializeField] private GameObject objectToSet;
    [SerializeField] private string parameterNameToSet;
    [SerializeField] private int valueToSet;
    [SerializeField] private float delayBeforeSet = 0f;
    [SerializeField] private float delayAfterSet = 0f;

    private Animator objectToCheckAnimator;
    private Animator objectToSetAnimator;
    private int originalValueToSet;
    private float delayBeforeSetTimer = 0f;
    private float delayAfterSetTimer = 0f;

    private void Start()
    {
        objectToCheckAnimator = objectToCheck.GetComponent<Animator>();
        objectToSetAnimator = objectToSet.GetComponent<Animator>();
        originalValueToSet = objectToSetAnimator.GetInteger(parameterNameToSet);
    }

    private void Update()
    {
        if (objectToCheckAnimator.GetInteger(parameterNameToCheck) == valueToCheck)
        {
            if (delayBeforeSetTimer <= 0f)
            {
                objectToSetAnimator.SetInteger(parameterNameToSet, valueToSet);
                delayAfterSetTimer = delayAfterSet;
            }
            else
            {
                delayBeforeSetTimer -= Time.deltaTime;
            }
        }
        else if (delayAfterSetTimer > 0f)
        {
            delayAfterSetTimer -= Time.deltaTime;
            if (delayAfterSetTimer <= 0f)
            {
                objectToSetAnimator.SetInteger(parameterNameToSet, originalValueToSet);
            }
        }
    }
}
