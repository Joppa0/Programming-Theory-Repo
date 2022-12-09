using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WinnerList : MonoBehaviour
{
    public static WinnerList instance { get; private set; }
    public string playerName;
    public int score;
    public string bestPlayer;
    public int bestScore;

    public void Awake()
    {
        // Checks if there is already an instance, if so, deletes the instance.
        if (instance != null)
        {
            Destroy(instance);
        }

        // Sets the instance and marks it to not be destroyed on load, meaning it will be saved between scenes.
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class SaveData
    {
        // The data to be saved.
        public string savePlayer;
        public int saveScore;
    }

    public void SaveWinnerData(string bestPlayer, int bestScore)
    {
        // Creates new data from the class SaveData.
        SaveData data = new SaveData();

        // Sets bestPlayer and bestScore to the name, and score inputs when the method was called.
        data.savePlayer = bestPlayer;
        data.saveScore = bestScore;

        // Saves all the data to a string.
        string json = JsonUtility.ToJson(data); 

        // Writes the data to the file path specified.
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadWinnerData()
    {
        // Finds the path where eventual winner data would be stored.
        string path = Application.persistentDataPath + "/savefile.json";

        // Reads the savedata if the file exists at the specified path.
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayer = data.savePlayer;
            bestScore = data.saveScore;
        }
    }
}
