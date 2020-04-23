using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for overall game control
// Assume semi-quavers, default 12 beat transitions
public class Control : MonoBehaviour
{
    public InputField bpmField;

    public int BPM = 600;

    private SoundActivator[] soundActivators;
    private float nextBeat = 0;
    private float secondsBetweenBeats;
    private bool simActive = false;

    void Start()
    {
        SetBPM(BPM);
    }

    void Update()
    {
        // Don't execute main beat loop if sim not active
        if (!simActive) return;

        // Wait until next beat
        if (Time.time >= nextBeat)
        {
            // Send out command to beat
            for (int i = 0; i < soundActivators.Length; ++i)
            {
                soundActivators[i].Beat();
            }

            nextBeat = Time.time + secondsBetweenBeats;
        }
    }

    // Set the BPM from the text field
    public void SetBPMFromField()
    {
        string text = bpmField.text;
        if (text != "") BPM = int.Parse(text);
        secondsBetweenBeats = 60f / BPM;
    }

    // Set BPM to given value
    public void SetBPM(int BPM)
    {
        this.BPM = BPM;
        secondsBetweenBeats = 60f / BPM;
        bpmField.text = "" + BPM;
    }

    // Start simulation
    public void StartSim()
    {
        simActive = true;
        soundActivators = GameObject.FindObjectsOfType<SoundActivator>();
    }

    // Stop simulation
    public void StopSim()
    {
        simActive = false;
    }

    // Save the game to xml
    public void SaveGame()
    {
        SavedGame savedGame = new SavedGame();
        savedGame.BuildFromScene();

        string path = Application.persistentDataPath + "/save.xml";
        SavedGame.WriteToFile(path, savedGame);
    }

    // Load the game from xml
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.xml";
        SavedGame savedGame = SavedGame.ReadFromFile(path);

        savedGame.PopulateScene();
    }
}
