using UnityEngine;

public class DisableObjectForCameras : MonoBehaviour
{
    private Camera[] allCameras;

    // Camera that uses this object as a render texture
    public Camera outputCamera;

    private void Start()
    {
        // Get all cameras in the scene
        allCameras = Camera.allCameras;

        // Disable rendering of this object for each camera in the scene, except the output camera
        foreach (var camera in allCameras)
        {
            if (camera != outputCamera)
            {
                camera.cullingMask &= ~(1 << gameObject.layer);
            }
        }
    }

    /*private void OnDestroy()
    {
        // Re-enable rendering of this object for each camera in the scene
        foreach (var camera in allCameras)
        {
            // Check if the camera is not destroyed before accessing it
            if (camera != null && camera != outputCamera)
            {
                camera.cullingMask |= 1 << gameObject.layer;
            }
        }
    } */
}
