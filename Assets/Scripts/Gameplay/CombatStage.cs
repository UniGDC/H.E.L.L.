using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatStage : AbstractGameplayStage
{
    public GameObject HomeworkPrefab;
    public GameObject TestPrefab;
    public GameObject FinalPrefab;

    public GameObject ClickCounter;
    public GameObject Player;

    public int LevelIndex;

    private int _currentClicks;
    private bool _clickGoalReached;

    public static readonly float VanishingPointYCoordinate = 7.5F;
    public float HomeworkSpeed = 1F;
    public float HomeworkInterval = 4F;
    public float TestInterval = 10F;
    public float EndTime = 60F;
    public bool SpawnFinal = true;

    // Use this for initialization
    public override void Begin()
    {
        base.Begin();

        if (HomeworkInterval > 0)
        {
            InvokeRepeating("LaunchHomework", HomeworkInterval, HomeworkInterval);
        }

        if (TestInterval > 0)
        {
            InvokeRepeating("LaunchTest", TestInterval, TestInterval);
        }

        Invoke("EndLevel", EndTime);
    }

    void LaunchHomework()
    {
        GameObject newHomework = Instantiate(HomeworkPrefab);

        // Assign position
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newHomework.transform.position = new Vector2(0, VanishingPointYCoordinate);

        // Assign target
        float deltaX = Player.transform.position.x; // Swap this out for Random.Range(-halfWidth, halfWidth) if you want randomized targeting.
        float deltaY = Player.transform.position.y - VanishingPointYCoordinate;
        Vector2 velocity = new Vector2(deltaX, deltaY).normalized;
        velocity.Scale(new Vector2(HomeworkSpeed, HomeworkSpeed));
        newHomework.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void LaunchTest()
    {
        GameObject newTest = Instantiate(TestPrefab);

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        newTest.transform.position = new Vector2(0, VanishingPointYCoordinate);
    }

    void EndLevel()
    {
        // Cancel all homework and test launching
        CancelInvoke();

        if (SpawnFinal)
        {
            GameObject finalExam = Instantiate(FinalPrefab);

            finalExam.transform.position = new Vector2(0,
                Camera.main.orthographicSize + finalExam.GetComponent<SpriteRenderer>().bounds.size.y / 2);

            FinalController controller = finalExam.GetComponent<FinalController>();
            controller.OnEnd = delegate { End(); };
        }
        else
        {
            End();
        }
    }
}