using UnityEngine;
using System.Collections;

public class LauncherScript : MonoBehaviour
{
    public GameObject HomeworkPrefab;
    public GameObject TestPrefab;
    public GameObject FinalPrefab;
    public GameObject Player;

    public float FinalStartTime = 60F;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("LaunchHomework", 5F, 4F);
        InvokeRepeating("LaunchTest", 6F, 6F);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LaunchHomework()
    {
        GameObject newHomework = Instantiate(HomeworkPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newHomework.transform.position = new Vector2(-halfWidth + Random.value * halfWidth * 2,
            Camera.main.orthographicSize + newHomework.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newHomework.GetComponent<HomeworkController>().Player = Player;
    }

    void LaunchTest()
    {
        GameObject newTest = Instantiate(TestPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newTest.transform.position = new Vector2(-halfWidth + Random.value * halfWidth * 2,
            Camera.main.orthographicSize + newTest.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newTest.GetComponent<TestController>().Player = Player;
    }

    void StartFinal()
    {
        // Cancel all homework and test launching
        CancelInvoke();

        GameObject finalTest = Instantiate(FinalPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        finalTest.transform.position = new Vector3(0,
            Camera.main.orthographicSize + finalTest.GetComponent<Sprite>().bounds.size.y / 2, 0);
    }
}