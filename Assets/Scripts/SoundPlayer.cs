using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public GameObject nextTarget;
    public int beatsFromTarget;
    public AudioClip sound;

    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = sound;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.transform == transform)
            {
                PlayAudio();
            }
        }
    }

    public void PlayAudio()
    {
        audio.Play();
    }
}
