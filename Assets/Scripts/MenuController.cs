using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text ButtonText;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TogglePause()
    {
        // Just an arbitrarily decided boundary.
        if (Time.timeScale <= 1E-5)
        {
            Time.timeScale = 1;
            ButtonText.text = "Pause";
        }
        else
        {
            Time.timeScale = 0;
            ButtonText.text = "Resume";
        }
    }
}