using UnityEngine;
using System.Collections;

public class LauncherScript : MonoBehaviour
{
    public GameObject HomeworkPrefab;
    public GameObject TestPrefab;
    public GameObject FinalPrefab;
    public GameObject Player;

    public float HomeworkInterval = 4F;
    public float TestInterval = 10F;
    public float FinalStartTime = 60F;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("LaunchHomework", HomeworkInterval, HomeworkInterval);
        InvokeRepeating("LaunchTest", TestInterval, TestInterval);
        Invoke("StartFinal", FinalStartTime);
    }

    // Update is called once per frame
    void Update()
    {
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