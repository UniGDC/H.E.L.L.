using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public class GameState : MonoBehaviour
{
    [Serializable]
    public class PlayerData
    {
        public int Level;
        public int Difficulty;
    }

    public static GameState State;

    public PlayerData Data = new PlayerData();

    // Use this for initialization
    void Awake()
    {
        // Assign state if not assigned
        if (State == null)
        {
            State = this;
            DontDestroyOnLoad(gameObject);
        }
        // THERE CAN ONLY BE ONE
        else if (State != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);

        bf.Serialize(fs,Data);
        fs.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            Data = (PlayerData) bf.Deserialize(fs);
            fs.Close();
        }
    }
}