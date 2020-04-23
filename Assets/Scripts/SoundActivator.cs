using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orbs to activate sounds
public class SoundActivator : SelectableObject, TargetableObject
{
    public GameObject initialTarget;
    public int beatsToInitialTarget;
    public bool active = false;

    GameObject target;
    Vector3 transformStep;

    private SoundPlayer sound;

    // Set initial target and calculate steps. TODO: Need to call after scene setup has changed
    void Start()
    {
        this.target = initialTarget;
        Vector3 gap = target.transform.position - transform.position;
        transformStep = gap / beatsToInitialTarget;
        sound = target.GetComponent<SoundPlayer>();
    }

    void Update ()
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
                    initialTarget = hitObject;
                }
            }
            else
            {
                // Set next target to nothing
                initialTarget = null;
            }
        }
    }

    // Move object on call of a beat
    public void Beat()
    {
        if (active)
        {
            if (!OnTarget()) transform.Translate(transformStep);
            if (OnTarget())
            {
                sound.PlayAudio();
                if (sound.nextTarget != null)
                {
                    SetTarget(sound.nextTarget);
                } else
                {
                    active = false;
                }
            }
        }
    }

    // Set the target and calculate steps
    private void SetTarget(GameObject target)
    {
        this.target = target;
        Vector3 gap = target.transform.position - transform.position;
        transformStep = gap / sound.beatsFromTarget;
        sound = target.GetComponent<SoundPlayer>();
    }

    private bool OnTarget()
    {
        return (target.transform.position - transform.position).magnitude < 0.001f;
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
