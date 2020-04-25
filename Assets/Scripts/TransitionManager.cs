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

    // Draw in all transition arrows
    public void DrawTransitions()
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

    // Draw in transitions after the update
    public void DrawTransitionsAfterUpdate()
    {
        StartCoroutine(TransitionDrawRoutine());
    }

    // Routine to draw transitions at end of frame
    private IEnumerator TransitionDrawRoutine()
    {
        yield return new WaitForEndOfFrame();
        DrawTransitions();
    }
}
