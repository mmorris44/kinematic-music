using UnityEngine;

// Plays a sound
public class SoundPlayer : SelectableObject, TransitionTarget
{
    public AudioClip sound;

    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = sound;
    }

    new void Update()
    {
        // Call base update
        base.Update();
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
