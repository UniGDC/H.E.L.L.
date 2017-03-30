﻿using System;
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

    public static readonly Vector2 VanishingPoint = new Vector2(0, 7.5F);

    public float HomeworkSpeedModifier;
    public float HomeworkInterval = 4F;
    public int HomeworkLaneCount = 7;
    public int HomeworkPackSize = 2;

    public float TestHorizontalSpeedModifier;
    public float TestVerticalSpeedModifier;
    public float TestTrackingStrength;
    public float TestInterval = 10F;

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
            InvokeRepeating("LaunchHomework", HomeworkInterval, HomeworkInterval);
        }
        if (TestInterval > 0)
        {
            InvokeRepeating("LaunchTest", TestInterval, TestInterval);
        }

        _startTime = Time.time;
        _homeworkCount = 0;
        _testCount = 0;

        _ending = false;
    }

    private void LaunchHomework()
    {
        _homeworkCount++;

        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        float targetY = PlayerController.Player.transform.position.y;
        float startY = Camera.main.orthographicSize + HomeworkPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // Assign target
        int[] targetIndices = MathFuncs.RandomDistictArray(HomeworkPackSize, 0, HomeworkLaneCount);
        foreach (int index in targetIndices)
        {
            float targetX = -halfWidth + halfWidth * (2 * index + 1) / HomeworkLaneCount;
            float startX = targetX / (VanishingPoint.y - targetY) * (VanishingPoint.y - startY);
            GameObject newHomework = Instantiate(HomeworkPrefab);

            newHomework.transform.position = new Vector3(startX, startY);
            newHomework.GetComponent<HomeworkController>().Parent = this;
            newHomework.GetComponent<HomeworkController>().Target = new Vector3(targetX, targetY);
            newHomework.GetComponent<HomeworkController>().SpeedModifier = HomeworkSpeedModifier;
        }

        if (_checkEnd())
        {
            _finishStage();
        }
    }

    private void LaunchTest()
    {
        _testCount++;

        GameObject newTest = Instantiate(TestPrefab);

        // Assign starting screenPosition
        float targetY = PlayerController.Player.transform.position.y;
        float startY = Camera.main.orthographicSize + TestPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        float targetX = PlayerController.Player.transform.position.x;
        float startX = targetX / (VanishingPoint.y - targetY) * (VanishingPoint.y - startY);

        newTest.transform.position = new Vector2(startX, startY);

        TestController controller = newTest.GetComponent<TestController>();
        controller.Parent = this;
        controller.HorizontalSpeed = TestHorizontalSpeedModifier;
        controller.VerticalSpeed = TestVerticalSpeedModifier;
        controller.TrackingStrength = TestTrackingStrength;

        if (_checkEnd())
        {
            _finishStage();
        }
    }

    private bool _checkEnd()
    {
        return TimeLimit > 0 && Time.time - _startTime >= TimeLimit || HomeworkLimit > 0 && _homeworkCount >= HomeworkLimit ||
               TestLimit > 0 && _testCount >= TestLimit;
    }

    private void _finishStage()
    {
        CancelInvoke();
        if (EndImmediately)
        {
            Invoke("End", EndDelay);
        }
        else
        {
            _ending = true;
        }
    }

    public void OnAssignmentDestroyed()
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