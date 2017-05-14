using System;
using UnityEngine;
using System.Collections;


public class LevelController : SingletonMonoBehaviour<LevelController>
{
    private int _currentIndex;
    public AbstractStage[] Stages;
    private Coroutine _stageCycler;

    private void Awake()
    {
        Instance = this;

        if (Stages == null || Stages.Length == 0)
        {
            Stages = gameObject.GetComponentsInChildren<AbstractStage>();
        }
        _currentIndex = 0;
    }

    private void Start()
    {
        _stageCycler = StartCoroutine(_runStages());
    }

    private IEnumerator _runStages()
    {
        for (; _currentIndex < Stages.Length; _currentIndex++)
        {
            Stages[_currentIndex].gameObject.SetActive(true);
            yield return StartCoroutine(Stages[_currentIndex].Run());
            Stages[_currentIndex].gameObject.SetActive(false);
        }
    }

//    public void EndCurrent()
//    {
//        _currentIndex++;
//
//        if (_currentIndex >= Stages.Length)
//        {
//            gameObject.SetActive(false);
//        }
//    }
//
//    public void StartNext()
//    {
//        if (_currentIndex < Stages.Length)
//        {
//            Stages[_currentIndex].ParentController = this;
//            Stages[_currentIndex].Run();
//        }
//        else
//        {
//            // No point keeping this canvas anymore
//            gameObject.SetActive(false);
//        }
//    }

    public void SkipToStage(int stageIndex)
    {
        StopCoroutine(_stageCycler);
        foreach (AbstractStage stage in Stages)
        {
            stage.Kill();
        }

        _currentIndex = stageIndex;
        _stageCycler = StartCoroutine(_runStages());
    }

//
//    public void Continue()
//    {
//        EndCurrent();
//        StartNext();
//    }
}