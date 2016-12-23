using UnityEngine;
using System.Collections;

public class HomeworkLauncher : MonoBehaviour
{
    public GameObject HomeworkPrefab;
    public GameObject Player;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("LaunchHomework", 5F, 4F);
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
            -Camera.main.orthographicSize - newHomework.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        newHomework.GetComponent<HomeworkController>().Player = Player;
    }
}