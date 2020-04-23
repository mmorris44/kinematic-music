using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages editing of the scene in-game
public class Creator : MonoBehaviour
{
    public GameObject arrowPrefab;

    private SelectableObject selectedObject;
    public List<Transition> transitions = new List<Transition>();

    void Start()
    {
        // Draw transitions at start
        DrawTransitions();
    }

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
    }
    
    // Draw in all transition arrows
    public void DrawTransitions()
    {
        // Destroy existing transitions
        foreach(Transition transition in transitions)
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
            // Check for sound activator
            SoundActivator soundActivator = musicObject.GetComponent<SoundActivator>();
            if (soundActivator != null)
            {
                transitionPairs.Add(new Tuple<GameObject, GameObject>(musicObject, soundActivator.initialTarget));
            }

            // Check for sound player
            SoundPlayer soundPlayer = musicObject.GetComponent<SoundPlayer>();
            if (soundPlayer != null)
            {
                transitionPairs.Add(new Tuple<GameObject, GameObject>(musicObject, soundPlayer.nextTarget));
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

    public void DrawTransitionsAfterUpdate()
    {
        StartCoroutine(TransitionDrawRoutine());
    }

    private IEnumerator TransitionDrawRoutine()
    {
        yield return new WaitForEndOfFrame();
        DrawTransitions();
    }

    // Deselects current selection, if any
    private void DeselectCurrent()
    {
        if (selectedObject != null) selectedObject.Deselect();
        selectedObject = null;
    }

}