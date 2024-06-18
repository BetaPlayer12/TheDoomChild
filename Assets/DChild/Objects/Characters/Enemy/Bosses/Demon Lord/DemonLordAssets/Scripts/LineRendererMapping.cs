using UnityEngine;

public class LineRendererMapping : MonoBehaviour
{
    public Transform pointA; // Reference to the first child GameObject
    public Transform pointB; // Reference to the second child GameObject

    public float lineWidthA = 0.2f; // Width of the line for pointA
    public float lineWidthB = 0.2f; // Width of the line for pointB

    private LineRenderer lineRenderer; // Reference to the LineRenderer component

    void Start()
    {
        // Get the LineRenderer component attached to this GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Ensure that both pointA and pointB are assigned
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA and PointB must be assigned in the inspector!");
            return;
        }

        // Set the initial positions and widths of the LineRenderer
        SetLineRendererProperties();
    }

    void SetLineRendererProperties()
    {
        // Set the positions of the LineRenderer points
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, pointA.position);
        lineRenderer.SetPosition(1, pointB.position);

        // Set the widths of the LineRenderer for pointA and pointB
        lineRenderer.startWidth = lineWidthA;
        lineRenderer.endWidth = lineWidthB;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the LineRenderer positions and widths every frame to ensure they stay updated
        SetLineRendererProperties();
    }
}
