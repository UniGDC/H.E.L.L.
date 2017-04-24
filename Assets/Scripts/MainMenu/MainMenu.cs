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

    public Scene[] LevelScenes;

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

    private void _toPlayScreen()
    {
        _checkSavedGame();
        PlayGameButton.SetActive(false);
        NewGameButton.SetActive(true);
        LoadGameButton.SetActive(true);
        WarnSaveOverrideScreen.SetActive(false);
    }

    private void _toWarnSaveOverrideScreen()
    {
        PlayGameButton.SetActive(false);
        NewGameButton.SetActive(false);
        LoadGameButton.SetActive(false);
        WarnSaveOverrideScreen.SetActive(true);
    }

    private void _toLevelScene(int level)
    {
        SceneManager.LoadScene(LevelScenes[level].buildIndex);
    }

    private void _toCurrentLevelScene()
    {
        _toLevelScene(GameState.Instance.Data.Level);
    }

    public void Menu() // TODO come up with a better name
    {
        _toMainScreen();
    }

    public void PlayGame()
    {
        _toPlayScreen();
    }

    public void NewGame(bool overrideSave)
    {
        if (!overrideSave && DataHandler.HasSaveFile())
        {
            _toWarnSaveOverrideScreen();
        }
        else
        {
            _resetGameStateData();
            _toCurrentLevelScene();
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
        _toCurrentLevelScene();
    }
}