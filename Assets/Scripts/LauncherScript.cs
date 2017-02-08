using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LauncherScript : MonoBehaviour
{
    public GameObject SimpleHomeworkPrefab;
    public GameObject HomeworkPrefab;
    public GameObject TestPrefab;
    public GameObject FinalPrefab;

    public GameObject ClickCounter;
    public GameObject Player;

    public int LevelIndex;
    /// <summary>
    /// The number of times user must click to start the game
    /// </summary>
    public int NumClicks = 10000000;

    private int _currentClicks;
    private bool _clickGoalReached;

    public float HomeworkInterval = 4F;
    public float TestInterval = 10F;
    public float FinalStartTime = 60F;

    // Use this for initialization
    void Start()
    {
        _currentClicks = 0;
        _clickGoalReached = false;

        switch (LevelIndex)
        {
            case 0:
            {
                break;
            }
            default:
            {
                InvokeRepeating("LaunchHomework", HomeworkInterval, HomeworkInterval);
                InvokeRepeating("LaunchTest", TestInterval, TestInterval);
                Invoke("StartFinal", FinalStartTime);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        if (!_clickGoalReached)
        {
            _currentClicks++;
            ClickCounter.GetComponent<Text>().text = "Current clicks: " + _currentClicks + "/" + NumClicks;
            if (_currentClicks >= NumClicks)
            {
                _clickGoalReached = true;
                ClickCounter.GetComponent<Text>().text = "Done!";
                InvokeRepeating("LaunchSimpleHomework", HomeworkInterval, HomeworkInterval);
            }
        }
    }

    void UpdateUI()
    {

    }

    void LaunchSimpleHomework()
    {
        GameObject newHomework = Instantiate(SimpleHomeworkPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newHomework.transform.position = new Vector2(Random.Range(-halfWidth, halfWidth),
            Camera.main.orthographicSize + newHomework.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newHomework.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1);
    }

    void LaunchHomework()
    {
        GameObject newHomework = Instantiate(HomeworkPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newHomework.transform.position = new Vector2(Random.Range(-halfWidth, halfWidth),
            Camera.main.orthographicSize + newHomework.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newHomework.GetComponent<HomeworkController>().Player = Player;
    }

    void LaunchTest()
    {
        GameObject newTest = Instantiate(TestPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newTest.transform.position = new Vector2(Random.Range(-halfWidth, halfWidth),
            Camera.main.orthographicSize + newTest.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newTest.GetComponent<TestController>().Player = Player;
    }

    void StartFinal()
    {
        // Cancel all homework and test launching
        CancelInvoke();

        GameObject finalTest = Instantiate(FinalPrefab);

        finalTest.GetComponent<FinalController>().Player = Player;

        finalTest.transform.position = new Vector2(0,
            Camera.main.orthographicSize + finalTest.GetComponent<SpriteRenderer>().bounds.size.y / 2);
    }
}