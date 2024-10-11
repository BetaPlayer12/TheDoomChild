using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRaycastPreVis : MonoBehaviour
{
    public float maxDistance = 100f;
    public float startLineWidth = 0.1f;
    public float endLineWidth = 0.1f;
    public LineRenderer lineRenderer;
    public string targetTag = "YourTagName";
    public LayerMask targetLayer;
    public GameObject attachedObject; // Ending attachment object
    public GameObject startAttachedObject; // Starting attachment object

    void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("Line Renderer not assigned. Please assign a Line Renderer in the Inspector.");
            return;
        }

        if (attachedObject == null)
        {
            Debug.LogError("Ending Attached Object not assigned. Please assign an object in the Inspector.");
            return;
        }

        if (startAttachedObject == null)
        {
            Debug.LogError("Starting Attached Object not assigned. Please assign an object in the Inspector.");
            return;
        }

        lineRenderer.startWidth = startLineWidth;
        lineRenderer.endWidth = endLineWidth;
    }

    void Update()
    {
        CastRaycast();
    }

    void CastRaycast()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, targetLayer);

        Vector2 endPoint;

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(targetTag))
            {
                endPoint = hit.point;
                UpdateAttachedObjectPosition(attachedObject, endPoint);
                attachedObject.SetActive(true);

                // Attach the starting object to the start point
                UpdateAttachedObjectPosition(startAttachedObject, origin);
                startAttachedObject.SetActive(true);
            }
            else
            {
                endPoint = origin + direction * maxDistance;
                attachedObject.SetActive(false);
                startAttachedObject.SetActive(false);
            }
        }
        else
        {
            endPoint = origin + direction * maxDistance;
            attachedObject.SetActive(false);
            startAttachedObject.SetActive(false);
        }

        DrawLine(origin, endPoint);
    }

    void DrawLine(Vector2 start, Vector2 end)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = startLineWidth;
        lineRenderer.endWidth = endLineWidth;
    }

    void UpdateAttachedObjectPosition(GameObject obj, Vector2 position)
    {
        obj.transform.position = position;
    }
}
