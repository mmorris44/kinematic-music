using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // Get all direct children of a game object
    public static GameObject[] ChildrenOf(GameObject gameObject)
    {
        GameObject[] children = new GameObject[gameObject.transform.childCount];
        for (int i = 0; i < children.Length; ++i)
        {
            children[i] = gameObject.transform.GetChild(i).gameObject;
        }
        return children;
    }
}
