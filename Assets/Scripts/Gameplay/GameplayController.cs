using System;
using UnityEngine;
using System.Collections;


public class GameplayController : MonoBehaviour
{
    public static GameplayController Controller;

    private int _currentIndex;
    public AbstractStage[] Stages;

    private void Awake()
    {
        if (Controller == null)
        {
            Controller = this;
        }
        else if (Controller != this)
        {
            Destroy(gameObject);
            return;
        }

        if (Stages == null || Stages.Length == 0)
        {
            Stages = gameObject.GetComponentsInChildren<AbstractStage>();
        }
        _currentIndex = 0;
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
            Stages[_currentIndex].Run();
        }
        else
        {
            // No point keeping this canvas anymore
            gameObject.SetActive(false);
        }
    }

    public void Continue()
    {
        EndCurrent();
        StartNext();
    }

    private void OnDestroy()
    {
        if (Controller == this)
        {
            Controller = null;
        }
    }
}