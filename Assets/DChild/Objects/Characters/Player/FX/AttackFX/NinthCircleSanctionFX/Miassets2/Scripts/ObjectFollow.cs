using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform targetObject; // The object to follow

    // Update is called once per frame
    void Update()
    {
        // Set the position of this object to match the position of the target object
        transform.position = targetObject.position;
    }
}
