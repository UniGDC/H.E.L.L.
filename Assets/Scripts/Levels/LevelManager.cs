using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public string[] LevelNames;

    public void ToLevelScene(int level)
    {
        SceneManager.LoadScene(LevelNames[level]);
    }

    public void ToCurrentLevelScene()
    {
        ToLevelScene(GameState.Instance.Data.Level);
    }
}