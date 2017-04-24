using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public class GameState : SingletonMonoBehaviour<GameState>
{
    [Serializable]
    public class PlayerData
    {
        public int Level; // 0-indexed.
        public int Difficulty; // TODO
        /// <summary>
        /// If true, when the player fails a level, the entire game is reset.
        /// </summary>
        public bool Hardcore;
    }

    public PlayerData Data = new PlayerData();

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }
}