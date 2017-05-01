using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public string MainMenuLevelName;
    public string[] LevelNames;

    private void Awake()
    {
        Instance = this;
    }

    public void ToLevelScene(int level)
    {
        SceneManager.LoadScene(LevelNames[level]);
    }

    public void ToCurrentLevelScene()
    {
        ToLevelScene(GameState.Instance.Data.Level);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(MainMenuLevelName);
    }
}