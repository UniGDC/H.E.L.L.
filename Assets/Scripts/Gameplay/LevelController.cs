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
        // Initialization
        Instance = this;
        _currentIndex = 0;
    }

    private void Start()
    {
        // Start cycling through the stages of the game
        _stageCycler = StartCoroutine(_runStages());
    }

    private IEnumerator _runStages()
    {
        for (; _currentIndex < Stages.Length; _currentIndex++)
        {
            Stages[_currentIndex].gameObject.SetActive(true);
            // Yielding a coroutine makes the super-coroutine wait for the sub-coroutine to finish before continuing
            yield return StartCoroutine(Stages[_currentIndex].Run());
            Stages[_currentIndex].gameObject.SetActive(false);
        }
    }

    public void SkipToStage(int stageIndex)
    {
        // Stop all stages
        StopCoroutine(_stageCycler);
        foreach (AbstractStage stage in Stages)
        {
            stage.Kill();
        }

        // Restart cycling through the stages at the desired index
        _currentIndex = stageIndex;
        _stageCycler = StartCoroutine(_runStages());
    }
}