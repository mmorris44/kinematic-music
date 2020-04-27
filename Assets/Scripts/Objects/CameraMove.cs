using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves the camera around
public class CameraMove : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float scrollSpeed = 1f;
    public float closestAllowed = 5f;

    private new Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        // Move around
        transform.Translate(Input.GetAxis("Horizontal") * Vector3.right * moveSpeed * Time.deltaTime);
        transform.Translate(Input.GetAxis("Vertical") * Vector3.up * moveSpeed * Time.deltaTime);

        // Zoom
        float zoomChange = Input.GetAxis("Mouse ScrollWheel") * -scrollSpeed * Time.deltaTime;
        float zoomNew = camera.orthographicSize + zoomChange;
        if (zoomNew > closestAllowed)
        {
            camera.orthographicSize = zoomNew;
        }
    }
}
