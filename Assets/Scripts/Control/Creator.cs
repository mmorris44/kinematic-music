using System.Collections.Generic;
using UnityEngine;

// Manages editing of the scene in-game
public class Creator : MonoBehaviour
{
    public GameObject multiSelectIconPrefab;
    private GameObject multiSelectIcon;

    private SelectableObject selectedObject;
    private List<SelectableObject> draggingObjects = new List<SelectableObject>();
    private TransitionManager transitionManager;
    private MouseControl mouseControl;
    private Vector3 lastMousePos = Vector3.back; // Equiv to null
    private bool multiSelect = false;

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
        if (mouseControl.LeftHeld() && draggingObjects.Count == 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(mouseControl.LeftMousePositionWhenDown(), Vector2.zero);

            // Check for hitting selectable object
            if (hit.transform != null && hit.transform.gameObject.GetComponent<SelectableObject>() != null)
            {
                draggingObjects.Add(hit.transform.gameObject.GetComponent<SelectableObject>());
            }
            else // Multiple selection
            {
                multiSelect = true;
                if (multiSelectIcon == null) multiSelectIcon = Instantiate(multiSelectIconPrefab);
            }
        }

        // Draw box for multi select
        if (multiSelect)
        {
            Vector3 corner1 = mouseControl.LeftMousePositionWhenDown();
            Vector3 corner2 = mouseControl.MousePosition();

            Vector2 center = (corner1 + corner2) / 2;
            Vector2 size = new Vector2(Mathf.Abs(corner1.x - corner2.x), Mathf.Abs(corner1.y - corner2.y));

            multiSelectIcon.transform.position = center;
            multiSelectIcon.transform.localScale = new Vector3(size.x, size.y);

            // Check for end of multi select
            if (!mouseControl.LeftHeld())
            {
                multiSelect = false;

                RaycastHit2D[] hitObjects = Physics2D.BoxCastAll(center, size, 0, Vector2.zero);
                foreach(RaycastHit2D hitObject in hitObjects)
                {
                    SelectableObject selectableObject = hitObject.transform.gameObject.GetComponent<SelectableObject>();
                    if (selectableObject != null) draggingObjects.Add(selectableObject);
                }
            }
        }

        // Check for no longer dragging object
        if (!mouseControl.LeftHeld() && draggingObjects.Count != 0)
        {
            foreach(SelectableObject draggingObject in draggingObjects)
            {
                transitionManager.UpdateTransitions(draggingObject.gameObject);
            }
            lastMousePos = Vector3.back; // Equiv to null

            // Clear dragging object if only one
            if (draggingObjects.Count == 1) DeselectCurrent();
        }

        // Drag objects around
        if (draggingObjects.Count != 0 && mouseControl.LeftHeld())
        {
            Vector3 mousePos = mouseControl.MousePosition();
            mousePos.z = 0;
            if (lastMousePos == Vector3.back) lastMousePos = mouseControl.LeftMousePositionWhenDown();

            foreach(SelectableObject draggingObject in draggingObjects)
            {
                draggingObject.transform.position += (mousePos - lastMousePos);
            }

            // Lag warning: Remove if game too laggy
            foreach (SelectableObject draggingObject in draggingObjects)
            {
                transitionManager.UpdateTransitions(draggingObject.gameObject);
            }

            if (multiSelectIcon != null) multiSelectIcon.transform.position += (mousePos - lastMousePos);

            lastMousePos = mousePos;
        }
    }

    // Deselects current selection, if any
    private void DeselectCurrent()
    {
        if (selectedObject != null) selectedObject.Deselect();
        selectedObject = null;

        // Get rid of dragging objects
        draggingObjects.Clear();
        if (multiSelectIcon != null) Destroy(multiSelectIcon);
    }

}