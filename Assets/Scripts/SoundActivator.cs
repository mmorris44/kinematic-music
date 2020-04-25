using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orbs to activate sounds
public class SoundActivator : SelectableObject
{
    public bool active = false;

    GameObject nextTarget;
    Vector3 transformStep;

    private SoundPlayer sound;

    // Set initial target and calculate steps. TODO: Need to call after scene setup has changed
    void Start()
    {
        this.nextTarget = target;
        Vector3 gap = nextTarget.transform.position - transform.position;
        transformStep = gap / beatsFromTarget;
        sound = nextTarget.GetComponent<SoundPlayer>();
    }

    new void Update ()
    {
        // Call base update
        base.Update();
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
                if (sound.target != null)
                {
                    SetTarget(sound.target);
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
        this.nextTarget = target;
        Vector3 gap = target.transform.position - transform.position;
        transformStep = gap / sound.beatsFromTarget;
        sound = target.GetComponent<SoundPlayer>();
    }

    // Check if on target
    private bool OnTarget()
    {
        return (nextTarget.transform.position - transform.position).magnitude < 0.001f;
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
