using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour
{
    public static readonly string SavePath = Application.persistentDataPath + "/save.dat";

    public static void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(SavePath, FileMode.OpenOrCreate);

        bf.Serialize(fs, GameState.Instance.Data);
        fs.Close();
    }

    public static void LoadData()
    {
        if (HasSaveFile())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(SavePath, FileMode.Open);

            GameState.Instance.Data = (GameState.PlayerData) bf.Deserialize(fs);
            fs.Close();
        }
    }

    public static bool HasSaveFile()
    {
        return File.Exists(SavePath);
    }
}