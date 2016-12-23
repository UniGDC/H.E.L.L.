using UnityEngine;
using System.Collections;

public class TestLauncher : MonoBehaviour
{
    public GameObject TestPrefab;
    public GameObject Player;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("LaunchTest", 6F, 6F);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LaunchTest()
    {
        GameObject newTest = Instantiate(TestPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newTest.transform.position = new Vector2(-halfWidth + Random.value * halfWidth * 2,
            -Camera.main.orthographicSize - newTest.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newTest.GetComponent<TestController>().Player = Player;
    }
}