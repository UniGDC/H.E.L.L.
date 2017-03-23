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

    public float HomeworkSpeedModifier;
    public float HomeworkInterval = 4F;
    public int HomeworkLaneCount = 7;
    public int HomeworkPackSize = 2;

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
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        float targetY = PlayerController.Player.transform.position.y;
        float startY = Camera.main.orthographicSize + HomeworkPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // Assign target
        int[] targetIndices = MathFuncs.RandomDistictArray(HomeworkPackSize, 0, HomeworkLaneCount);
        foreach (int index in targetIndices)
        {
            float targetX = -halfWidth + halfWidth * (2 * index + 1) / HomeworkLaneCount;
            float startX = targetX / (VanishingPointYCoordinate - targetY) * (VanishingPointYCoordinate - startY);

            GameObject newHomework = Instantiate(HomeworkPrefab);

            newHomework.transform.position = new Vector3(startX, startY);
            newHomework.GetComponent<HomeworkController>().Target = new Vector3(targetX, targetY);
            newHomework.GetComponent<HomeworkController>().SpeedModifier = HomeworkSpeedModifier;
        }
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