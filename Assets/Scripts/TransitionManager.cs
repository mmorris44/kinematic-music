using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    private List<Transition> transitions = new List<Transition>();

    void Start()
    {
        // Draw transitions at start
        DrawTransitions();
    }

    // Draw in transitions after the update
    public void DrawTransitionsAfterUpdate()
    {
        StartCoroutine(TransitionDrawRoutine());
    }

    // Updates the transforms of all transitions involving the given game object
    public void UpdateTransitions(GameObject gameObject)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.from == gameObject || transition.to == gameObject) transition.UpdateTransform();
        }
    }

    // Deletes all transitions involving the current game object
    public void DeleteGameobject(GameObject gameObject)
    {
        // TODO: implement
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
        GameObject arrowRoot = GameObject.Find("/Arrows");
        foreach (Tuple<GameObject, GameObject> transition in transitionPairs)
        {
            GameObject transitionObject = Instantiate(arrowPrefab);
            transitionObject.GetComponent<Transition>().SetTransition(transition.Item1, transition.Item2);
            transitionObject.transform.parent = arrowRoot.transform;
            transitions.Add(transitionObject.GetComponent<Transition>());
        }
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
