using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : SelectableObject
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

    public void PlayAudio()
    {
        audio.Play();
    }

    public override void Select()
    {
        selected = true;
    }

    public override void Deselect()
    {
        selected = false;
    }
}
