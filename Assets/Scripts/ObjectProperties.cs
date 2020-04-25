using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any selectable object
public abstract class SelectableObject : MonoBehaviour
{
    // Compulsory members
    public bool selected = false;

    // Non-compulsory members
    public GameObject target = null;
    public int beatsFromTarget = -1;

    protected void Update()
    {
        // If right click while selected
        if (Input.GetMouseButtonDown(1) && selected)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Check for hit object
            if (hit.transform != null)
            {
                // Check for targetable object
                GameObject hitObject = hit.transform.gameObject;
                TransitionTarget targetableObject = hitObject.GetComponent<TransitionTarget>();
                if (targetableObject != null)
                {
                    target = hitObject;
                }
            }
            else
            {
                // Set next target to nothing
                target = null;
            }
        }
    }

    public abstract void Select();
    public abstract void Deselect();
}

public interface TransitionTarget { } // Eligible to be target of a transition