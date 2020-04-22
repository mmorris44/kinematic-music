using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        SetBPM();
    }

    void Update()
    {
        if (!simActive) return;

        if (Time.time >= nextBeat)
        {
            for (int i = 0; i < soundActivators.Length; ++i)
            {
                soundActivators[i].Beat();
            }

            nextBeat = Time.time + secondsBetweenBeats;
        }
    }

    public void SetBPM()
    {
        string text = bpmField.text;
        if (text != "") BPM = int.Parse(text);
        secondsBetweenBeats = 60f / BPM;
    }

    public void StartSim()
    {
        simActive = true;
        soundActivators = GameObject.FindObjectsOfType<SoundActivator>();
    }

    public void StopSim()
    {
        simActive = false;
    }

    public void SaveGame()
    {
        SavedGame savedGame = new SavedGame();
        savedGame.BuildFromScene();

        string path = Application.persistentDataPath + "/save.xml";
        SavedGame.WriteToFile(path, savedGame);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.xml";
        SavedGame savedGame = SavedGame.ReadFromFile(path);

        savedGame.PopulateScene();
    }
}
