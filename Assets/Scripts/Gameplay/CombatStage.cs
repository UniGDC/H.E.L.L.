using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatStage : AbstractStage
{
    public GameObject HomeworkPrefab;
    public GameObject TestPrefab;
    public GameObject FinalPrefab;

    public static readonly Vector2 VanishingPoint = new Vector2(0, 7.5F);

    [Serializable]
    public class HomeworkSpawnerConfig
    {
        public float Interval;
        public int PackSize;
        public int LaneCount = 7;
        public float SpeedModifier;
    }

    [Serializable]
    public class TestSpawnerConfig
    {
        public float Interval;
        public float HorizontalSpeedModifier;
        public float VerticalSpeedModifier;
        public float TrackingStrength;
        public float DisableTrackingY;
    }

    private HomeworkSpawnerConfig _homeworkConfig;
    private TestSpawnerConfig _testConfig;

    [Serializable]
    public class SpawnerConfig
    {
        public float Time;
        public HomeworkSpawnerConfig HomeworkConfig;
        public TestSpawnerConfig TestConfig;
    }

    public SpawnerConfig[] Configs;

    public float EndDelay;

    private Coroutine _homeworkSpawner;
    private Coroutine _testSpawner;
    private bool _spawning;
    private bool _running;

    // Use this for initialization
    public override IEnumerator Run()
    {
        _running = true;
        _spawning = true;
        // Start the config updating coroutine
        StartCoroutine(_startUpdate());

        // Start homework and test spawning coroutine
        _homeworkSpawner = StartCoroutine(_homeworkCoroutine());
        _testSpawner = StartCoroutine(_testCoroutine());

        yield return new WaitWhile(() => _running);
    }

    private IEnumerator _startUpdate()
    {
        // Use a foreach to loop through all the configs one by one
        foreach (SpawnerConfig config in Configs)
        {
            _homeworkConfig = config.HomeworkConfig;
            _testConfig = config.TestConfig;
            // Wait for the given amount of time, and then continue with the loop to access the next config
            yield return new WaitForSeconds(config.Time);
        }
        // TODO
        StartCoroutine(_finishStage());
    }

    private IEnumerator _homeworkCoroutine()
    {
        while (_spawning)
        {
            // If period is negative, pause.
            yield return new WaitWhile(() => _homeworkConfig == null || _homeworkConfig.Interval <= 0);
            yield return new WaitForSeconds(_homeworkConfig.Interval);
            LaunchHomework();
        }
    }

    private void LaunchHomework()
    {
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        float targetY = PlayerController.Instance.gameObject.transform.position.y;
        float startY = Camera.main.orthographicSize + HomeworkPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // Assign target
        int[] targetIndices = MathFuncs.RandomDistictArray(_homeworkConfig.PackSize, 0, _homeworkConfig.LaneCount);
        foreach (int index in targetIndices)
        {
            float targetX = -halfWidth + halfWidth * (2 * index + 1) / _homeworkConfig.LaneCount;
            float startX = targetX / (VanishingPoint.y - targetY) * (VanishingPoint.y - startY);
            GameObject newHomework = Instantiate(HomeworkPrefab);
            newHomework.transform.parent = Spawned.Instance.gameObject.transform;

            newHomework.transform.position = new Vector3(startX, startY);
            newHomework.GetComponent<HomeworkController>().Parent = this;
            newHomework.GetComponent<HomeworkController>().Target = new Vector3(targetX, targetY);
            newHomework.GetComponent<HomeworkController>().SpeedModifier = _homeworkConfig.SpeedModifier;
        }
    }

    private IEnumerator _testCoroutine()
    {
        while (_spawning)
        {
            // If period is negative, pause.
            yield return new WaitWhile(() => _testConfig == null || _testConfig.Interval <= 0);
            yield return new WaitForSeconds(_testConfig.Interval);
            LaunchTest();
        }
    }

    private void LaunchTest()
    {
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
        controller.HorizontalSpeed = _testConfig.HorizontalSpeedModifier;
        controller.VerticalSpeed = _testConfig.VerticalSpeedModifier;
        controller.TrackingStrength = _testConfig.TrackingStrength;
        controller.DisableTrack = _testConfig.DisableTrackingY;
    }

    private IEnumerator _finishStage()
    {
        _spawning = false;
        yield return new WaitWhile(() => GameObject.FindGameObjectWithTag("Assignment") != null);
        _running = false;
    }

    public override void Kill()
    {
        StopAllCoroutines();
        _spawning = false;
        _running = false;
        gameObject.SetActive(false);
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