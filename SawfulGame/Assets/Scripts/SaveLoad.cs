using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static HighScores highScores = new HighScores();

    public static bool saveFileExists = false;

    public static bool loaded = false;


    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highScores.gd");
        bf.Serialize(file, highScores);
        file.Close();
    }

    public static bool Load()
    {
        if(File.Exists(Application.persistentDataPath+"/highScores.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highScores.gd", FileMode.Open);
            highScores = (HighScores)bf.Deserialize(file);
            file.Close();
            saveFileExists = true;
            return true;
        }
        else
        {
            saveFileExists = false;
            highScores = new HighScores();
            return false;
        }
    }
}
