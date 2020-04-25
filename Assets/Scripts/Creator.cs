using UnityEngine;

// Manages editing of the scene in-game
public class Creator : MonoBehaviour
{
    private SelectableObject selectedObject;
    private SelectableObject draggingObject;
    private TransitionManager transitionManager;
    private MouseControl mouseControl;

    void Start()
    {
        transitionManager = GetComponent<TransitionManager>();
        mouseControl = GetComponent<MouseControl>();
    }

    void Update()
    {
        // Check for selection click
        if (mouseControl.LeftClick())
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseControl.LeftMousePositionWhenDown(), Vector2.zero);

            // Check for hit object
            if (hit.transform != null)
            {
                GameObject hitObject = hit.transform.gameObject;

                // Check for selectable object
                SelectableObject selectableObject = hitObject.GetComponent<SelectableObject>();
                if (selectableObject != null)
                {
                    DeselectCurrent();
                    selectedObject = selectableObject;
                    selectedObject.Select();
                }

                // Check for sound player to make noise
                SoundPlayer soundPlayer = hitObject.GetComponent<SoundPlayer>();
                if (soundPlayer != null)
                {
                    soundPlayer.PlayAudio();
                }
            } else
            {
                // Deselect if nothing hit
                DeselectCurrent();
            }
        }

        // Check for target click
        if (mouseControl.RightClick() && selectedObject != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseControl.RightMousePositionWhenDown(), Vector2.zero);

            // Check for hit object
            if (hit.transform != null)
            {
                transitionManager.SetTransition(selectedObject.gameObject, hit.transform.gameObject);
            }
            else
            {
                // Set next target to nothing
                transitionManager.SetTransition(selectedObject.gameObject, null);
            }
        }

        // Check for deletion
        if (Input.GetKeyDown(KeyCode.Delete) && selectedObject != null)
        {
            Destroy(selectedObject.gameObject);
            transitionManager.DeleteTransitionsForGameObject(selectedObject.gameObject);
            selectedObject = null;
        }

        // Check if holding mouse over object
        if (mouseControl.LeftHeld() && draggingObject == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseControl.LeftMousePositionWhenDown(), Vector2.zero);

            // Check for hitting selectable object
            if (hit.transform != null && hit.transform.gameObject.GetComponent<SelectableObject>() != null)
            {
                draggingObject = hit.transform.gameObject.GetComponent<SelectableObject>();
            }
        }

        // Check for no longer dragging object
        if (!mouseControl.LeftHeld() && draggingObject)
        {
            transitionManager.UpdateTransitions(draggingObject.gameObject);
            draggingObject = null;
        }

        // Drag object around
        if (draggingObject != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            draggingObject.transform.position = mousePos;
        }
    }

    // Deselects current selection, if any
    private void DeselectCurrent()
    {
        if (selectedObject != null) selectedObject.Deselect();
        selectedObject = null;
    }

}