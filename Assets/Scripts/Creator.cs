﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages editing of the scene in-game
public class Creator : MonoBehaviour
{
    SelectableObject selectedObject;

    void Update()
    {
        // Check for selection click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Check for hit object
            if (hit.transform != null)
            {
                GameObject hitObject = hit.transform.gameObject;

                // Check for selectable object
                {
                    SelectableObject selectableObject = hitObject.GetComponent<SelectableObject>();
                    if (selectableObject != null)
                    {
                        DeselectCurrent();
                        selectedObject = selectableObject;
                        selectedObject.Select();
                    }
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
    }

    // Deselects current selection, if any
    private void DeselectCurrent()
    {
        if (selectedObject != null) selectedObject.Deselect();
        selectedObject = null;
    }

}