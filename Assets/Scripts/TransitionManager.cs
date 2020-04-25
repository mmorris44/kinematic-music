using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public GameObject arrowPrefab;

    private List<Transition> transitions = new List<Transition>();
    private GameObject arrowRoot;

    void Start()
    {
        arrowRoot = GameObject.Find("/Arrows");

        // Draw transitions at start
        DrawTransitions();
    }

    // Draw in transitions after the update
    public void DrawTransitionsAfterUpdate()
    {
        StartCoroutine(TransitionDrawRoutine());
    }

    // Updates the transforms of all transitions involving the given game object
    // e.g. after object moves
    public void UpdateTransitions(GameObject gameObject)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.from == gameObject || transition.to == gameObject) transition.UpdateTransform();
        }
    }

    // Deletes all transitions involving the current game object
    // e.g. after object deleted
    public void DeleteTransitionsForGameObject(GameObject gameObject)
    {
        foreach (Transition transition in transitions)
        {
            // Remove transitions that go from selectable objects to given game object
            if (transition.to == gameObject)
            {
                SelectableObject selectableObject = transition.from.GetComponent<SelectableObject>();
                selectableObject.target = null;
            }

            // Destroy transition objects that involve the game object
            if (transition.from == gameObject || transition.to == gameObject) Destroy(transition.gameObject);
        }

        // Remove transitions involving game object from the list
        transitions.RemoveAll(transition => transition.from == gameObject || transition.to == gameObject);
    }

    // Sets transition from given game object. Must be selectable
    // e.g. no transition, change transition, create transition
    public void SetTransition(GameObject from, GameObject to)
    {
        // Delete existing transition arrow from object
        foreach (Transition transition in transitions)
        {
            if (transition.from == from) Destroy(transition.gameObject);
        }

        // Set target and delete existing transition from object
        from.GetComponent<SelectableObject>().target = to;
        transitions.RemoveAll(transition => transition.from == from);

        // Add new transition if 'to' exists
        if (to != null)
        {
            CreateTransition(from, to);
        }
    }

    // Draw in all transition arrows
    private void DrawTransitions()
    {
        // Destroy existing transitions
        foreach (Transition transition in transitions)
        {
            Destroy(transition.gameObject);
        }
        transitions.Clear();

        // Build pairs of transitions
        List<Tuple<GameObject, GameObject>> transitionPairs = new List<Tuple<GameObject, GameObject>>();
        GameObject[] musicObjects = Utils.ChildrenOf(GameObject.Find("/Music"));

        // Check objects for transitions to add
        foreach (GameObject musicObject in musicObjects)
        {
            // Check for all selectable objects
            SelectableObject selectableObject = musicObject.GetComponent<SelectableObject>();

            // Check that target exists
            if (selectableObject != null && selectableObject.target != null)
            {
                transitionPairs.Add(new Tuple<GameObject, GameObject>(musicObject, selectableObject.target));
            }
        }

        // Draw transitions
        foreach (Tuple<GameObject, GameObject> transition in transitionPairs)
        {
            CreateTransition(transition.Item1, transition.Item2);
        }
    }

    // Create new transition. Checks if valid transition target
    private void CreateTransition(GameObject from, GameObject to)
    {
        // Check if valid transition target
        TransitionTarget transitionTarget = to.GetComponent<TransitionTarget>();
        if (transitionTarget == null)
        {
            Debug.Log("Object " + to.GetHashCode() + " is not a valid transition target");
            return;
        }

        // Create transition
        GameObject transitionObject = Instantiate(arrowPrefab);
        transitionObject.GetComponent<Transition>().SetTransition(from, to);
        transitionObject.transform.parent = arrowRoot.transform;
        transitions.Add(transitionObject.GetComponent<Transition>());
    }

    // Updates the location and dimensions of an existing transition
    private void UpdateTransition(GameObject from, GameObject to)
    {
        Transition transition = null;
        foreach(Transition el in transitions)
        {
            if (el.from == from && el.to == to)
            {
                transition = el;
                break;
            }
        }

        // If transition not found in list, there must be an error in the code
        if (transition == null)
        {
            Debug.LogError("Could not find transition between gameobjects " + from.GetHashCode() + " and " + to.GetHashCode());
            return;
        }

        transition.SetTransition(from, to);
    }

    // Routine to draw transitions at end of frame
    private IEnumerator TransitionDrawRoutine()
    {
        yield return new WaitForEndOfFrame();
        DrawTransitions();
    }
}
