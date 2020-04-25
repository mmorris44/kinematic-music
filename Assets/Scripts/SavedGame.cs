using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class SavedGame
{
    public int BPM;
    public SaveSoundActivator[] soundActivators;
    public SaveSoundPlayer[] soundPlayers;

    // Build the saved game object from the scene
    public void BuildFromScene()
    {
        // Fetch relevant data from scene
        BPM = GameObject.Find("Control").GetComponent<Control>().BPM;
        GameObject[] gameObjects = Utils.ChildrenOf(GameObject.Find("/Music"));

        List<SaveSoundActivator> saveSoundActivators = new List<SaveSoundActivator>();
        List<SaveSoundPlayer> saveSoundPlayers = new List<SaveSoundPlayer>();

        // Go through each game object
        for (int i = 0; i < gameObjects.Length; ++i)
        {
            // Check for sound activators
            SoundActivator soundActivator = gameObjects[i].GetComponent<SoundActivator>();
            if (soundActivator != null)
            {
                saveSoundActivators.Add(
                    new SaveSoundActivator(i, soundActivator.transform.position, System.Array.IndexOf(gameObjects, soundActivator.initialTarget),
                    soundActivator.beatsToInitialTarget, soundActivator.active));
            }

            // Check for sound players
            SoundPlayer soundPlayer = gameObjects[i].GetComponent<SoundPlayer>();
            if (soundPlayer != null)
            {
                saveSoundPlayers.Add(
                    new SaveSoundPlayer(i, soundPlayer.transform.position, System.Array.IndexOf(gameObjects, soundPlayer.nextTarget),
                    soundPlayer.beatsFromTarget, soundPlayer.sound.name));
            }
        }

        // Convert to arrays
        soundActivators = saveSoundActivators.ToArray();
        soundPlayers = saveSoundPlayers.ToArray();
    }

    // Populate the scene based on data in object
    public void PopulateScene()
    {
        // Clear existing music
        GameObject musicObject = GameObject.Find("/Music");
        GameObject[] currentObjects = Utils.ChildrenOf(musicObject);
        foreach(GameObject gameObject in currentObjects)
        {
            Object.Destroy(gameObject);
        }

        // Keep track of <id, gameobject> pairs
        Dictionary<int, GameObject> gameObjectDictionary = new Dictionary<int, GameObject>();

        // Set BPM
        GameObject controlObject = GameObject.Find("Control");
        controlObject.GetComponent<Control>().SetBPM(BPM);

        // Load prefabs
        GameObject soundActivatorObject = (GameObject) Resources.Load("Prefabs/sound_activator");
        GameObject soundPlayerObject = (GameObject)Resources.Load("Prefabs/sound_player");

        // Instantiate sound activators
        foreach (SaveSoundActivator saveSoundActivator in soundActivators) {
            GameObject gameObject = Object.Instantiate(soundActivatorObject);
            gameObjectDictionary.Add(saveSoundActivator.id, gameObject);
            gameObject.transform.position = saveSoundActivator.GetPosition();
            gameObject.transform.parent = musicObject.transform;

            SoundActivator soundActivator = gameObject.GetComponent<SoundActivator>();
            soundActivator.beatsToInitialTarget = saveSoundActivator.beatsToInitialTarget;
            soundActivator.active = saveSoundActivator.active;
        }

        // Instantiate sound players
        foreach (SaveSoundPlayer saveSoundPlayer in soundPlayers)
        {
            GameObject gameObject = Object.Instantiate(soundPlayerObject);
            gameObjectDictionary.Add(saveSoundPlayer.id, gameObject);
            gameObject.transform.position = saveSoundPlayer.GetPosition();
            gameObject.transform.parent = musicObject.transform;

            SoundPlayer soundPlayer = gameObject.GetComponent<SoundPlayer>();
            soundPlayer.beatsFromTarget = saveSoundPlayer.beatsFromTarget;
            soundPlayer.sound = (AudioClip) Resources.Load("Audio/" + saveSoundPlayer.audioClipName);
        }

        // Link sound activators
        foreach (SaveSoundActivator saveSoundActivator in soundActivators)
        {
            GameObject gameObject;
            gameObjectDictionary.TryGetValue(saveSoundActivator.id, out gameObject);

            GameObject other;
            if (gameObjectDictionary.TryGetValue(saveSoundActivator.initialTarget, out other))
            {
                gameObject.GetComponent<SoundActivator>().initialTarget = other;
            } else { Debug.Log("[ERROR] While populating scene. Could not find referenced object with ID " + saveSoundActivator.initialTarget); }
        }

        // Link sound players
        foreach (SaveSoundPlayer saveSoundPlayer in soundPlayers)
        {
            GameObject gameObject;
            gameObjectDictionary.TryGetValue(saveSoundPlayer.id, out gameObject);

            GameObject other;
            if (gameObjectDictionary.TryGetValue(saveSoundPlayer.nextTarget, out other))
            {
                gameObject.GetComponent<SoundPlayer>().nextTarget = other;
            }
            else { Debug.Log("[ERROR] While populating scene. Could not find referenced object with ID " + saveSoundPlayer.nextTarget); }
        }

        // Draw transitions
        controlObject.GetComponent<TransitionManager>().DrawTransitionsAfterUpdate();
    }

    // Write the given saved game to a file
    public static void WriteToFile(string path, SavedGame savedGame)
    {
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log("Saving file to " + path);

        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SavedGame));
        x.Serialize(stream, savedGame);
        stream.Close();
    }

    // Return a saved game build from the given file
    public static SavedGame ReadFromFile(string path)
    {
        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SavedGame));
        StreamReader reader = new StreamReader(path);
        SavedGame savedGame = (SavedGame) x.Deserialize(reader);
        reader.Close();
        return savedGame;
    }
}

// Generic object that can be saved
[System.Serializable]
public class SaveObject
{
    public int id;
    public float[] position;

    public SaveObject()
    {
        id = -1;
        position = new float[] { -1, -1, -1 };
    }

    public SaveObject(int id, Vector3 position)
    {
        this.id = id;
        this.position = new float[3]{position.x, position.y, position.z};
    }

    public Vector3 GetPosition()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}

// Save sound activator
[System.Serializable]
public class SaveSoundActivator : SaveObject
{
    public int initialTarget;
    public int beatsToInitialTarget;
    public bool active;

    public SaveSoundActivator() : base()
    {
        initialTarget = -1;
        beatsToInitialTarget = -1;
        active = false;
    }

    public SaveSoundActivator(int id, Vector3 position, int initialTarget, int beatsToInitialTarget, bool active) : base(id, position)
    {
        this.initialTarget = initialTarget;
        this.beatsToInitialTarget = beatsToInitialTarget;
        this.active = active;
    }
}

// Save sound player
[System.Serializable]
public class SaveSoundPlayer : SaveObject
{
    public int nextTarget;
    public int beatsFromTarget;
    public string audioClipName;

    public SaveSoundPlayer() : base()
    {
        nextTarget = -1;
        beatsFromTarget = -1;
        audioClipName = "-1";
    }

    public SaveSoundPlayer(int id, Vector3 position, int nextTarget, int beatsFromTarget, string audioClipName) : base(id, position)
    {
        this.nextTarget = nextTarget;
        this.beatsFromTarget = beatsFromTarget;
        this.audioClipName = audioClipName;
    }
}