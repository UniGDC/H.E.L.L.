using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject PlayGameButton;
    public GameObject NewGameButton;
    public GameObject LoadGameButton;
    public GameObject WarnSaveOverrideScreen;

    private void Start()
    {
        _checkSavedGame();
        _toMainScreen();
    }

    private void _checkSavedGame()
    {
        LoadGameButton.GetComponent<Button>().interactable = DataHandler.HasSaveFile();
    }

    private void _toMainScreen()
    {
        PlayGameButton.SetActive(true);
        NewGameButton.SetActive(false);
        LoadGameButton.SetActive(false);
        WarnSaveOverrideScreen.SetActive(false);
    }

    public void ToPlayScreen()
    {
        _checkSavedGame();
        PlayGameButton.SetActive(false);
        NewGameButton.SetActive(true);
        LoadGameButton.SetActive(true);
        WarnSaveOverrideScreen.SetActive(false);
    }

    public void ToWarnSaveOverrideScreen()
    {
        PlayGameButton.SetActive(false);
        NewGameButton.SetActive(false);
        LoadGameButton.SetActive(false);
        WarnSaveOverrideScreen.SetActive(true);
    }

    public void NewGame(bool overrideSave)
    {
        if (!overrideSave && DataHandler.HasSaveFile())
        {
            ToWarnSaveOverrideScreen();
        }
        else
        {
            _resetGameStateData();
            LevelManager.Instance.ToCurrentLevelScene();
        }
    }

    private void _resetGameStateData()
    {
        GameState.Instance.Data = new GameState.PlayerData();
        DataHandler.SaveData();
    }

    public void LoadGame()
    {
        DataHandler.LoadData();
        LevelManager.Instance.ToCurrentLevelScene();
    }
}