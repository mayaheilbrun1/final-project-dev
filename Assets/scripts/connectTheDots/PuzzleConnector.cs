using System.Collections.Generic;
using UnityEngine;

public class PuzzleConnector : MonoBehaviour
{
    public GameObject linePrefab;

    private LineRenderer currentLine;
    private Transform startDot;
    private string startName;
    private Transform hoveredHiddenDot;

    // Define correct input-to-hidden connections
    public Dictionary<string, string> correctConnections = new Dictionary<string, string>
    {
        { "Input0", "Hidden2" },
        { "Input1", "Hidden0" },
        { "Input2", "Hidden1" }
    };

    private HashSet<string> connectedInputs = new HashSet<string>();

    void Update()
    {
        if (currentLine != null)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            currentLine.SetPosition(1, mouseWorld);
        }
    }

    public void StartConnection(Transform dot)
    {
        if (connectedInputs.Contains(dot.name)) return;

        startDot = dot;
        startName = dot.name;

        GameObject lineObj = Instantiate(linePrefab);
        currentLine = lineObj.GetComponent<LineRenderer>();
        currentLine.SetPosition(0, startDot.position);
        currentLine.SetPosition(1, startDot.position);
    }

    public void HoveredOverHiddenDot(Transform hiddenDot)
    {
        hoveredHiddenDot = hiddenDot;
    }

    public bool IsConnecting()
    {
        return currentLine != null;
    }

    public void TryEndConnection()
    {
        if (currentLine == null || startDot == null || hoveredHiddenDot == null)
        {
            CancelConnection();
            return;
        }

        string endName = hoveredHiddenDot.name;

        if (correctConnections.ContainsKey(startName) && correctConnections[startName] == endName)
        {
            currentLine.SetPosition(1, hoveredHiddenDot.position);
            currentLine.startColor = Color.green;
            currentLine.endColor = Color.green;

            hoveredHiddenDot.GetComponent<Dot>().LockColor();  // Lock in green
            connectedInputs.Add(startName);
            CheckWin();
        }
        else
        {
            // ❗ Reset yellow color back to white here:
            hoveredHiddenDot.GetComponent<Dot>().ResetColor();

            Destroy(currentLine.gameObject); // Incorrect connection
        }

        currentLine = null;
        startDot = null;
        hoveredHiddenDot = null;
    }


    private void CancelConnection()
    {
        if (currentLine != null)
            Destroy(currentLine.gameObject);

        currentLine = null;
        startDot = null;
        hoveredHiddenDot = null;
    }

    private void CheckWin()
    {
        if (connectedInputs.Count == correctConnections.Count)
        {
            Debug.Log("🎉 Puzzle Complete!");
        }
    }
}
