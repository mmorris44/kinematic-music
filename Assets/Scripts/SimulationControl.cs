using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for overall game control
// Assume semi-quavers, default 12 beat transitions
public class SimulationControl : MonoBehaviour
{
    public InputField bpmField;
    public Text startButtonText;
    public Button resetButton;

    public int BPM = 600;

    private SoundActivator[] soundActivators;
    private float nextBeat = 0;
    private float secondsBetweenBeats;
    private bool simActive = false;
    private bool simFresh = true;

    private SaveControl saveControl;

    void Start()
    {
        // Try to load game from save file
        saveControl = GetComponent<SaveControl>();
        saveControl.LoadGame(saveControl.reservedFilename);

        resetButton.interactable = false;
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

    // Start button pressed
    public void OnStartButtonPressed()
    {
        if (simActive)
        {
            startButtonText.text = "Start";
            PauseSim();
        }
        else
        {
            startButtonText.text = "Pause";
            StartSim();
            resetButton.interactable = true;
        }
    }

    // Reset button pressed
    public void OnResetButtonPressed()
    {
        PauseSim();
        startButtonText.text = "Start";
        simFresh = true;
        resetButton.interactable = false;
        saveControl.LoadGame(saveControl.reservedFilename);
    }

    // Check if sim is active
    public bool SimInProgress()
    {
        return !simFresh;
    }

    // Start simulation
    private void StartSim()
    {
        simActive = true;
        if (simFresh)
        {
            soundActivators = GameObject.FindObjectsOfType<SoundActivator>();
            saveControl.SaveGame(saveControl.reservedFilename);
        }

        simFresh = false;
    }

    // Pause simulation
    private void PauseSim()
    {
        simActive = false;
    }
}
