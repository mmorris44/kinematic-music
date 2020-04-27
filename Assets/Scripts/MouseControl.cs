using UnityEngine;

// Track mouse movement and clicks
public class MouseControl : MonoBehaviour
{
    public float holdThreshold = 0.3f; // Amount of time to click to register as a hold

    // Mouse state tracking. 0,1,2 = left,right,middle
    private bool[] mouseDown = new bool[3];
    private float[] mouseHoldStart = new float[3];
    private bool[] mouseClicked = new bool[3];
    private bool[] mouseHeld = new bool[3];
    private Vector2[] mousePositionWhenDown = new Vector2[3];

    void Start()
    {
        // Set initial mouse state
        for (int i = 0; i < 3; ++i)
        {
            mouseDown[i] = Input.GetMouseButton(i);
        }
    }

    void Update()
    {
        // Set click to false
        for (int i = 0; i < 3; ++i)
        {
            mouseClicked[i] = false;
        }

        // Check if mouse goes active
        for (int i = 0; i < 3; ++i)
        {
            // Record when mouse when down
            if (!mouseDown[i] && Input.GetMouseButton(i))
            {
                mouseDown[i] = true;
                mouseHoldStart[i] = Time.time;
                mousePositionWhenDown[i] = MousePosition();
            }
        }

        // Check if mouse has been active long enough to be held
        for (int i = 0; i < 3; ++i)
        {
            if (mouseDown[i] && Time.time - mouseHoldStart[i] >= holdThreshold)
            {
                mouseHeld[i] = true;
            }
        }

        // Check if mouse goes inactive
        for (int i = 0; i < 3; ++i)
        {
            if (mouseDown[i] && !Input.GetMouseButton(i))
            {
                mouseDown[i] = false;
                if (Time.time - mouseHoldStart[i] < holdThreshold) // Clicked
                {
                    mouseClicked[i] = true;
                } else // Held end
                {
                    mouseHeld[i] = false;
                }
            }
        }
    }

    public Vector3 MousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool LeftClick()
    {
        return mouseClicked[0];
    }

    public bool RightClick()
    {
        return mouseClicked[1];
    }

    public bool LeftHeld()
    {
        return mouseHeld[0];
    }

    public Vector2 LeftMousePositionWhenDown()
    {
        return mousePositionWhenDown[0];
    }

    public Vector2 RightMousePositionWhenDown()
    {
        return mousePositionWhenDown[1];
    }
}