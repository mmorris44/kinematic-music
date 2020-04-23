using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plays a sound
public class SoundPlayer : SelectableObject, TargetableObject
{
    public GameObject nextTarget;
    public int beatsFromTarget;
    public AudioClip sound;

    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = sound;
    }

    void Update()
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
                TargetableObject targetableObject = hitObject.GetComponent<TargetableObject>();
                if (targetableObject != null)
                {
                    nextTarget = hitObject;
                }
            } else
            {
                // Set next target to nothing
                nextTarget = null;
            }
        }
    }

    public void PlayAudio()
    {
        audio.Play();
    }

    public override void Select()
    {
        selected = true;
    }

    public override void Deselect()
    {
        selected = false;
    }
}
