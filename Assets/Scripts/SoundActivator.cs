using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundActivator : MonoBehaviour
{
    public GameObject initialTarget;
    public int beatsToInitialTarget;
    public bool active = false;

    GameObject target;
    Vector3 transformStep;

    private SoundPlayer sound;

    private void Start()
    {
        this.target = initialTarget;
        Vector3 gap = target.transform.position - transform.position;
        transformStep = gap / beatsToInitialTarget;
        sound = target.GetComponent<SoundPlayer>();
    }

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
}
