using UnityEngine;
using System.Collections;

public class HomeworkLauncher : MonoBehaviour
{
    public GameObject HomeworkPrefab;
    public GameObject Player;
    public float Speed;

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
        newHomework.transform.position += new Vector3(-3 + Random.value * 6, 0, 0);

        newHomework.GetComponent<Rigidbody2D>().velocity =
            (Player.transform.position - newHomework.transform.position).normalized * Speed;
        Destroy(newHomework, 10F);
    }
}