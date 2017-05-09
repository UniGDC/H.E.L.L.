using System;
using UnityEngine;
using System.Collections;


public class LevelController : SingletonMonoBehaviour<LevelController>, IStageController
{
    private int _currentIndex;
    public AbstractStage[] Stages;

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
        StartNext();
    }

    public void EndCurrent()
    {
        _currentIndex++;

        if (_currentIndex >= Stages.Length)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartNext()
    {
        if (_currentIndex < Stages.Length)
        {
            Stages[_currentIndex].ParentController = this;
            Stages[_currentIndex].Begin();
        }
        else
        {
            // No point keeping this canvas anymore
            gameObject.SetActive(false);
        }
    }

    public void SkipToStage(int stageIndex)
    {
        foreach (AbstractStage stage in Stages)
        {
            stage.gameObject.SetActive(false);
        }
        _currentIndex = stageIndex;
        StartNext();
    }

    public void Continue()
    {
        EndCurrent();
        StartNext();
    }
}