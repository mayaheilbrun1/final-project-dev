using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [Header("Line Settings")]
    public Material lineMaterial;
    public float lineWidth = 0.1f;

    [Header("Drawing Area")]
    public SpriteRenderer drawingArea;  // 📌 הספרייט שתחום בו הציור

    private LineRenderer _currentLineRenderer;
    private List<Vector3> _pointsList;

    void Start()
    {
        _pointsList = new List<Vector3>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            if (IsInsideDrawingArea(mouseWorldPos))
            {
                CreateNewLine();
                AddPoint(mouseWorldPos);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();

            if (_currentLineRenderer != null && IsInsideDrawingArea(mouseWorldPos))
            {
                if (_pointsList.Count == 0 || Vector3.Distance(_pointsList[_pointsList.Count - 1], mouseWorldPos) > 0.1f)
                {
                    AddPoint(mouseWorldPos);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_pointsList.Count > 0)
                Debug.Log("Finished drawing stroke with " + _pointsList.Count + " points.");
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f; // מרחק מהמצלמה
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    bool IsInsideDrawingArea(Vector3 worldPos)
    {
        if (drawingArea == null) return true; // אם לא הוגדר – לא מגביל
        return drawingArea.bounds.Contains(worldPos);
    }

    void CreateNewLine()
    {
        GameObject lineObj = new GameObject("Line");
        _currentLineRenderer = lineObj.AddComponent<LineRenderer>();
        _currentLineRenderer.material = lineMaterial;
        _currentLineRenderer.startWidth = lineWidth;
        _currentLineRenderer.endWidth = lineWidth;
        _currentLineRenderer.positionCount = 0;
        _currentLineRenderer.numCapVertices = 5;

        _pointsList.Clear();
    }

    void AddPoint(Vector3 point)
    {
        _pointsList.Add(point);
        _currentLineRenderer.positionCount = _pointsList.Count;
        _currentLineRenderer.SetPositions(_pointsList.ToArray());
    }
}
