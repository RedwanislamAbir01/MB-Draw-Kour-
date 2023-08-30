using _Game.Managers;
using System.Collections.Generic;
using _Game;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class path : MonoBehaviour
{
    [SerializeField] private GameObject _drawingSpritePrefab;
    [SerializeField] private LineFollower _playerLineFollower;
    [SerializeField] private float _lineYOffset;
    [SerializeField] private bool _canDrawLine;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _targetDrawingLayer;

    private LineRenderer _lineRenderer;
    private List<Vector3> _pointsList;
    private GameObject _currentDrawingSprite;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
       
        _pointsList = new List<Vector3>();
    }
    private void Start()
    {
        GameManager.Instance.OnEolTrigger += DisableLineFollower;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnEolTrigger -= DisableLineFollower;
    }
    void DisableLineFollower() { _playerLineFollower.enabled = false; ResetPath(); }
    private void Update()
    {
        if (_canDrawLine && !_playerLineFollower.endPointReached)
        {
            // You can add logic here if needed for continuous path updates
        }

        if (PlayerState.Instance.IsMoving()) return;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _currentDrawingSprite = Instantiate(_drawingSpritePrefab, transform.position,  _drawingSpritePrefab.transform.rotation);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _playerLayer))
            {
                if (hit.transform.CompareTag("Player"))
                {
                   
                    Time.timeScale = 0.0f;
                    _canDrawLine = true;
                    _lineRenderer.enabled = true;
                }
            }
        }
        
        if (Input.GetKey(KeyCode.Mouse0) && _canDrawLine)
        {
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _targetDrawingLayer))
            {
                const float pointGapThreshold = 0.5f;
              
                if (DistanceToLastPoint(hit.point) > pointGapThreshold)
                {
                 
                    // var adjustedPoint = new Vector3(hit.point.x, 0.025f, hit.point.z);
                    var adjustedPoint = new Vector3(hit.point.x, hit.point.y + _lineYOffset, hit.point.z);
                    _pointsList.Add(adjustedPoint);

                    _lineRenderer.positionCount = _pointsList.Count;
                    _lineRenderer.SetPositions(_pointsList.ToArray());

                    _currentDrawingSprite.transform.position = adjustedPoint;
                    // LOL
                    LineSmoother.SmoothLine(_pointsList.ToArray(), 0.1f); // Not sure what this function does 
                }
            }
        }
        else
        {
            Destroy(_currentDrawingSprite);
            _currentDrawingSprite = null;
            CompletePathCreation();
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        return _pointsList.Count == 0 ? Mathf.Infinity : Vector3.Distance(_pointsList[^1], point);
    }

    private void CompletePathCreation()
    {
        // Notify listeners about the new path
        OnNewPathCreated(_pointsList);
        _currentDrawingSprite = null; Destroy(_currentDrawingSprite);
        // Enable or disable the behavior based on lineRenderer's state
        _playerLineFollower.enabled = _lineRenderer.enabled;

        Time.timeScale = 1f;
    }

    private void OnNewPathCreated(List<Vector3> newPathPoints)
    {
        // You can implement your logic here when a new path is created
    }

    public void ResetPath()
    {
        _pointsList.Clear();
        _lineRenderer.positionCount = 0;
        _canDrawLine = false;
        _lineRenderer.enabled = false;
    }
}
