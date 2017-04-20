using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject StartGameButton;
    public GameObject NewGameButton;
    public GameObject LoadGameButton;

    public void StartGame()
    {
        StartGameButton.SetActive(false);

        NewGameButton.SetActive(true);
        LoadGameButton.GetComponent<Button>().interactable = DataLoader.HasSaveFile();
        LoadGameButton.SetActive(true);
    }
}