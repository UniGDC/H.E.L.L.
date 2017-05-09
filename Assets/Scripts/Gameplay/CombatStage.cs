using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatStage : AbstractStage
{
    public GameObject HomeworkPrefab;
    public GameObject TestPrefab;
    public GameObject FinalPrefab;

    private int _currentClicks;
    private bool _clickGoalReached;

    public static readonly Vector2 VanishingPoint = new Vector2(0, 7.5F);

    public float HomeworkSpeedModifier;
    public float HomeworkInterval;
    public int HomeworkLaneCount = 7;
    public int HomeworkPackSize;

    public float TestHorizontalSpeedModifier;
    public float TestVerticalSpeedModifier;
    public float TestTrackingStrength;
    public float TestDisableTrackingY;
    public float TestInterval;

    // End conditions. The combat stage will end if any one of these set conditions is met.
    public float TimeLimit;

    private float _startTime;
    public int HomeworkLimit;
    private int _homeworkCount;
    public int TestLimit;
    private int _testCount;

    public bool EndImmediately = false;
    public float EndDelay;
    private bool _ending;

    // Use this for initialization
    public override void Begin()
    {
        base.Begin();

        if (HomeworkInterval > 0)
        {
//            InvokeRepeating("LaunchHomework", HomeworkInterval, HomeworkInterval);
            StartCoroutine(_homeworkCoroutine());
        }
        if (TestInterval > 0)
        {
//            InvokeRepeating("LaunchTest", TestInterval, TestInterval);
            StartCoroutine(_testCoroutine());
        }

        _startTime = Time.time;
        _homeworkCount = 0;
        _testCount = 0;

        _ending = false;

        StartCoroutine(_checkEndTime());
    }

    private IEnumerator _homeworkCoroutine()
    {
        while (HomeworkLimit == 0 || _homeworkCount < HomeworkLimit)
        {
            yield return new WaitForSeconds(HomeworkInterval);
            LaunchHomework();
        }
        _finishStage();
    }

    private void LaunchHomework()
    {
        _homeworkCount++;

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        float targetY = PlayerController.Instance.gameObject.transform.position.y;
        float startY = Camera.main.orthographicSize + HomeworkPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // Assign target
        int[] targetIndices = MathFuncs.RandomDistictArray(HomeworkPackSize, 0, HomeworkLaneCount);
        foreach (int index in targetIndices)
        {
            float targetX = -halfWidth + halfWidth * (2 * index + 1) / HomeworkLaneCount;
            float startX = targetX / (VanishingPoint.y - targetY) * (VanishingPoint.y - startY);
            GameObject newHomework = Instantiate(HomeworkPrefab);
            newHomework.transform.parent = Spawned.Instance.gameObject.transform;

            newHomework.transform.position = new Vector3(startX, startY);
            newHomework.GetComponent<HomeworkController>().Parent = this;
            newHomework.GetComponent<HomeworkController>().Target = new Vector3(targetX, targetY);
            newHomework.GetComponent<HomeworkController>().SpeedModifier = HomeworkSpeedModifier;
        }
    }

    private IEnumerator _testCoroutine()
    {
        while (TestLimit == 0 || _testCount < TestLimit)
        {
            yield return new WaitForSeconds(TestInterval);
            LaunchTest();
        }
        _finishStage();
    }

    private void LaunchTest()
    {
        _testCount++;

        GameObject newTest = Instantiate(TestPrefab);
        newTest.transform.parent = Spawned.Instance.gameObject.transform;

        // Assign starting screenPosition
        float targetY = PlayerController.Instance.gameObject.transform.position.y;
        float startY = Camera.main.orthographicSize + TestPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float targetX = PlayerController.Instance.gameObject.transform.position.x;
        float startX = targetX / (VanishingPoint.y - targetY) * (VanishingPoint.y - startY);

        newTest.transform.position = new Vector2(startX, startY);

        TestController controller = newTest.GetComponent<TestController>();
        controller.Parent = this;
        controller.HorizontalSpeed = TestHorizontalSpeedModifier;
        controller.VerticalSpeed = TestVerticalSpeedModifier;
        controller.TrackingStrength = TestTrackingStrength;
        controller.DisableTrack = TestDisableTrackingY;
    }

    private IEnumerator _checkEndTime()
    {
        while (TimeLimit <= 0 || Time.time - _startTime < TimeLimit)
        {
            yield return new WaitForSeconds(0.5F);
        }
        _finishStage();
    }

    private void _finishStage()
    {
        StopAllCoroutines();
        if (EndImmediately)
        {
            Invoke("End", EndDelay);
        }
        else
        {
            _ending = true;
            CheckEnd();
        }
    }

    public void CheckEnd()
    {
        if (_ending && GameObject.FindWithTag("Assignment") == null)
        {
            // No more assignments on screen, end.
            Invoke("End", EndDelay);
        }
    }

    public override void End()
    {
        base.End();
    }

    /// <summary>
    /// Constructs a 4x4 matrix to transform a vector on the floor to a vector in the pseudo-3D 2D game.
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public static Matrix4x4 GetPerspectiveTransformationMatrix(Vector2 screenPosition)
    {
        // Only work with 2D
        Vector2 toVanishingPoint = new Vector2(-screenPosition.x, VanishingPoint.y - screenPosition.y);
        float scale = toVanishingPoint.y / VanishingPoint.y;
        toVanishingPoint.Scale(new Vector2(1 / VanishingPoint.y, 1 / VanishingPoint.y));

        Matrix4x4 transformationMatrix = new Matrix4x4();
        // Transformed i
        transformationMatrix[0, 0] = scale;
        // Transformed j
        transformationMatrix.SetColumn(1, toVanishingPoint);
        transformationMatrix[2, 2] = 1;
        transformationMatrix[3, 3] = 1;
        return transformationMatrix;
    }
}