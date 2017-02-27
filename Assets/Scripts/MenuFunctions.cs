using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    public Button[] LoadSaveButtons;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadGameSaves(Button ToHide)
    {
        for (int i = 0; i < LoadSaveButtons.Length; i++)
        {
            int saveSlot = i;
            if (File.Exists(Application.persistentDataPath + "/save" + (saveSlot + 1) + ".dat"))
            {
                LoadSaveButtons[i].GetComponent<Button>().onClick.AddListener(delegate { LoadGame(saveSlot); });
            }
            else
            {
                LoadSaveButtons[i].GetComponentInChildren<Text>().text = "New Game";

                LoadSaveButtons[i].GetComponent<Button>().onClick.AddListener(delegate { NewGame(saveSlot); });
            }

            LoadSaveButtons[i].GetComponent<Image>().enabled = true;
            LoadSaveButtons[i].GetComponentInChildren<Text>().enabled = true;
        }

        ToHide.GetComponent<Image>().enabled = false;
        ToHide.GetComponentInChildren<Text>().enabled = false;
    }

    public void NewGame(int saveSlot)
    {
        GameState.State.SaveSlot = saveSlot;
        GameState.State.Save();
    }

    public void LoadGame(int saveSlot)
    {
        GameState.State.Load(saveSlot);
    }

    public void TogglePause(Text buttonText)
    {
        // Just an arbitrarily decided boundary.
        if (Time.timeScale <= 1E-5)
        {
            Time.timeScale = 1;
            buttonText.text = "Pause";
        }
        else
        {
            Time.timeScale = 0;
            buttonText.text = "Resume";
        }
    }
}