using System;
using UnityEngine.Analytics;

public class GameState : SingletonMonoBehaviour<GameState>
{
    [Serializable]
    public class PlayerData
    {
        public Gender PlayerSex;

        public int Level; // 0-indexed.
        public int Difficulty; // TODO

        /// <summary>
        /// If true, when the player fails a level, the entire game is reset.
        /// </summary>
        public bool Hardcore;

        public int Kudos;
    }

    public PlayerData Data = new PlayerData();

    // Use this for initialization
    private void Awake()
    {
        Instance = this;
    }
}