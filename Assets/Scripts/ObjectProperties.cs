using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : MonoBehaviour
{
    public bool selected = false;

    public abstract void Select();
    public abstract void Deselect();
}

public interface TargetableObject { }