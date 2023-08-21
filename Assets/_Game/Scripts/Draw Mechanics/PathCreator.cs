using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class path : MonoBehaviour
{
    public MonoBehaviour behavior;
    public LineRenderer lineRenderer;
    public bool drawLine = false;
    public LayerMask excludedLayers;

    private List<Vector3> points = new List<Vector3>();
    private LineFollower lineFollower = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Start()
    {
        lineFollower = behavior.GetComponent<LineFollower>();
    }

    private void Update()
    {
        if (drawLine && !lineFollower.endPointReached)
        {
            // You can add logic here if needed for continuous path updates
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Use layer mask to exclude specific layers like "Enemy"
            int layerMask = ~excludedLayers;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Time.timeScale = 0.0f;
                    drawLine = true;
                    lineRenderer.enabled = true;
                }

                if (DistanceToLastPoint(hit.point) > 0.5f && drawLine)
                {
                    Vector3 adjustedPoint = new Vector3(hit.point.x, 0.025f, hit.point.z);
                    points.Add(adjustedPoint);

                    lineRenderer.positionCount = points.Count;
                    lineRenderer.SetPositions(points.ToArray());
                    LineSmoother.SmoothLine(points.ToArray(), 0.1f); // Not sure what this function does
                }
            }
        }
        else
        {
            CompletePathCreation();
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (points.Count == 0)
            return Mathf.Infinity;

        return Vector3.Distance(points[points.Count - 1], point);
    }

    private void CompletePathCreation()
    {
        // Notify listeners about the new path
        OnNewPathCreated(points);

        // Enable or disable the behavior based on lineRenderer's state
        behavior.enabled = lineRenderer.enabled;

        Time.timeScale = 1f;
    }

    private void OnNewPathCreated(List<Vector3> newPathPoints)
    {
        // You can implement your logic here when a new path is created
    }

    public void ResetPath()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
        drawLine = false;
        lineRenderer.enabled = false;
    }
}
