using UnityEngine;

public class Dot : MonoBehaviour
{
    public PuzzleConnector connector;

    private bool isInput;
    private SpriteRenderer sr;
    private bool isLocked = false;

    void Start()
    {
        isInput = gameObject.name.StartsWith("Input");
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (isInput)
            connector.StartConnection(transform);
    }

    void OnMouseEnter()
    {
        if (!isInput && connector.IsConnecting() && !isLocked)
        {
            connector.HoveredOverHiddenDot(transform);
            sr.color = Color.yellow;
        }
    }

    void OnMouseExit()
    {
        if (!isInput && connector.IsConnecting() && !isLocked)
        {
            connector.HoveredOverHiddenDot(null);
            sr.color = Color.white;
        }
    }

    void OnMouseUp()
    {
        if (isInput && connector.IsConnecting())
        {
            connector.TryEndConnection();
        }
    }

    public void LockColor()
    {
        isLocked = true;
        sr.color = Color.green;
    }

    public void ResetColor()
    {
        if (!isLocked)
            sr.color = Color.white;
    }
}