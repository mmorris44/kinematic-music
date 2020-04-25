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

    public abstract void Select();
    public abstract void Deselect();
}

public interface TransitionTarget { } // Eligible to be target of a transition