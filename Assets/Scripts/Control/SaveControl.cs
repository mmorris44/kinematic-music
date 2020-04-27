using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Control for saving and loading game
public class SaveControl : MonoBehaviour
{
    public string reservedFilename;
    public string extension;

    public Text filenameText;

    private SimulationControl simulationControl;

    void Start()
    {
        simulationControl = GetComponent<SimulationControl>();
    }

    // Save game clicked
    public void OnSaveGameClick()
    {
        if (ValidGameState())
        {
            SaveGame(filenameText.text);
            SaveGame(reservedFilename);
        }
    }

    // Load game clicked
    public void OnLoadGameClick()
    {
        if (ValidGameState())
        {
            LoadGame(filenameText.text);
            SaveGame(reservedFilename);
        }
    }

    // Save the game to xml
    public void SaveGame(string filename)
    {
        SavedGame savedGame = new SavedGame();
        savedGame.BuildFromScene();

        string path = Application.persistentDataPath + "/" + filename + extension;
        SavedGame.WriteToFile(path, savedGame);

        if (filename != reservedFilename) Debug.Log("File saved to " + path);
    }

    // Load the game from xml
    public void LoadGame(string filename)
    {
        string path = Application.persistentDataPath + "/" + filename + extension;
        SavedGame savedGame = SavedGame.ReadFromFile(path);

        savedGame.PopulateScene();

        if (filename != reservedFilename) Debug.Log("File loaded from " + path);
    }

    // Check if game state is valid for saving / loading
    private bool ValidGameState()
    {
        if (filenameText.text == "")
        {
            Debug.Log("Cannot load/save: Filename cannot be empty");
            return false;
        }
        if (filenameText.text == reservedFilename)
        {
            Debug.Log("Cannot load/save: Filename is reserved");
            return false;
        }
        if (simulationControl.SimInProgress())
        {
            Debug.Log("Cannot load/save: Simulation in progress");
            return false;
        }
        return true;
    }
}
