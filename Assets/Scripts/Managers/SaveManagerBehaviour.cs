using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManagerBehaviour
{
    static SaveData[] allLevels;

    public static void Save(SaveData data)
    {
        if (allLevels == null)
        {
            LoadLevels();
            if (allLevels == null)
            {
                allLevels = new SaveData[4];
            }
        }

        for (int i = 0; i < allLevels.Length; ++i) 
        {
            if (allLevels[i] == null || 
                allLevels[i].GetLevelName() == data.GetLevelName())
            {
                allLevels[i] = data;
                break;
            }
        }

        string folderPath = Application.persistentDataPath + "\\Saves";
        string fileName = "save.happycook";

        if (!Directory.Exists(folderPath)) 
        {
            Directory.CreateDirectory(folderPath);
        }

        FileStream file = new FileStream(folderPath + "\\" + fileName, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, allLevels);
        file.Close();
    }

    public static SaveData Load(string levelName)
    {
        if (allLevels == null)
        {
            LoadLevels();
        }

        if (allLevels != null)
        {
            // Search in the array
            for (int i = 0; i < allLevels.Length; ++i)
            {
                if (allLevels[i] !=null && allLevels[i].GetLevelName() == levelName)
                {
                    return allLevels[i];
                }
            }
        }

        return null;

    }

    private static void LoadLevels()
    {
        string folderPath = Application.persistentDataPath + "\\Saves";
        string fileName = "save.happycook";
        string path = folderPath + "\\" + fileName;

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            allLevels = (SaveData[])formatter.Deserialize(file);
            file.Close();
        }
    }
}

[System.Serializable]
public class SaveData
{
    string levelName;
    int score;
    int highestCombo;
    int fails;
    int delivers;
    int stars;

    public SaveData(string levelName, int score, int highestCombo, int fails, int delivers, int stars)
    {
        this.levelName = levelName;
        this.score = score;
        this.highestCombo = highestCombo;
        this.fails = fails;
        this.delivers = delivers;
        this.stars = stars;
    }

    public string GetLevelName() {  return levelName; }
    public int GetScore() { return score; }
    public int GetHighestCombo() { return highestCombo; }
    public int GetDelivers() { return delivers; }
    public int GetStars() {  return stars; }
    public int GetFailures() { return fails; }

}
